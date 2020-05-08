using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Reflection;
using System.ComponentModel.Design;
using System.Configuration;
using System.Diagnostics;
using System.Xml;

namespace ARMO_Test_solution
{
	public partial class Form1 : Form
	{
		const string PLACEHOLDER_FILENAME = "Регулярное выражение для названия файла";//".*\\.ini";
		const string PLACEHOLDER_FINDSTRING = "Строка поиска";//"1";
		
		const int LENGTH = 512 * 1024;

		int secCnt;
		int totalFiles;
		int findedFiles;
		int calculatedFiles;

		byte[] bytesUTF8;
		byte[] bytesASCII;

		string workDir;
		string workStr;
		string workPat;

		object lockerEvent = new object();
		object lockerStr = new object();
		object lockerNum = new object();

		string liveFile;

		Thread thread;

		List<EventHandlerArgsAddMethod> evList = new List<EventHandlerArgsAddMethod>();
		
		public event EventHandler CallClear;
		public event EventHandler CallTreeChange;

		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			var elseAction = new Action(() =>
			{
				findStrTextBox.Text = PLACEHOLDER_FINDSTRING;
				fileNameTextBox.Text = PLACEHOLDER_FILENAME;
				folderTextBox.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
				findStrTextBox.GotFocus += TextBox_Focus;
				fileNameTextBox.GotFocus += TextBox_Focus;
			});

			if (File.Exists("state.ini"))
				try 
				{
					using (var file = new StreamReader("state.ini"))
					{
						folderTextBox.Text = file.ReadLine();
						fileNameTextBox.Text = file.ReadLine();
						findStrTextBox.Text = file.ReadLine();
					}
				}
				catch 
				{
					elseAction();
				}
			else
			{
				elseAction();
			}
			
			timer.Interval = 10;
			timerData.Interval = 200;
			
			timerData.Tick += timerData_Tick;
			CallClear += form1_Clear;
			treeView1.AfterSelect += (s, ev) => rezPanel.Controls.Clear();
			CallTreeChange += Form1_UpdateTree;

			folderTextBox.TextChanged += this.TextChangedSave;
			fileNameTextBox.TextChanged += this.TextChangedSave;
			findStrTextBox.TextChanged += this.TextChangedSave;
			
			fileNameTextBox.LostFocus += TextBox_LostFocus;
			findStrTextBox.LostFocus += TextBox_LostFocus;
			timer.Tick += timer_Tick;

			rezPanel.AutoScroll = true;
		}

		/// <summary>
		/// Проверят, не остался ли textBox пустым, после действий пользователя
		/// </summary>
		private void TextBox_LostFocus(object sender, EventArgs e)
		{
			if (((TextBox)sender).Text == "")
			{
				if (((TextBox)sender).Name == findStrTextBox.Name)
				{
					((TextBox)sender).Text = "Любая срока";
				}
				else if (((TextBox)sender).Name == fileNameTextBox.Name)
				{
					((TextBox)sender).Text = "Любое регулярное выражение, например .*\\.ini";
				}
			}
		}

		/// <summary>
		/// Удаляет подсказку при первом исполльзовании
		/// </summary>
		private void TextBox_Focus(object sender, EventArgs e)
		{
			if (((TextBox)sender).Text == PLACEHOLDER_FILENAME || ((TextBox)sender).Text == PLACEHOLDER_FINDSTRING)
			{
				((TextBox)sender).GotFocus -= TextBox_Focus;
				((TextBox)sender).Text = String.Empty;
			}
			((TextBox)sender).ForeColor = Color.Black;
			((TextBox)sender).Font = new Font("Microsoft Sans Serif", 8.5f, FontStyle.Regular);
		}

		/// <summary>
		/// Выполняет переданное дейсвие
		/// </summary>
		private void Form1_UpdateTree(object sender, EventArgs e)
		{
			
			((EventHandlerArgsAddMethod)e).ActionToRightTHread();
		}

		/// <summary>
		/// Очищает дерево элементов
		/// </summary>
		private void form1_Clear(object sender, EventArgs e)
		{
			treeView1.Nodes.Clear();
		}


		/// <summary>
		/// Продолжает выполнение потока поиска
		/// </summary>
		private void stopButtonContinue_Click(object sender, EventArgs e)
		{
			thread.Resume();
			timer.Start();
			timerData.Start();
			findButton.Enabled = false;
			((Button)sender).Text = "Стоп";
			((Button)sender).Click += stopButton_Click;
			((Button)sender).Click -= stopButtonContinue_Click;
		}

		/// <summary>
		/// Останавливает поток поиска
		/// </summary>
		private void stopButton_Click(object sender, EventArgs e)
		{
			thread.Suspend();
			timer.Stop();
			timerData.Stop();
			findButton.Enabled = true;
			((Button)sender).Text = "Продолжить";
			((Button)sender).Click -= stopButton_Click;
			((Button)sender).Click += stopButtonContinue_Click;
		}


		private void opendirButton_Click(object sender, EventArgs e)
		{
			if (folderBrowserDialog.ShowDialog() == DialogResult.Cancel)
				return;
			folderTextBox.Text = folderBrowserDialog.SelectedPath;
		}

		/// <summary>
		/// Выполняет сохранение данных, введённых пользователем
		/// </summary>
		private void TextChangedSave(object sender, EventArgs e)
		{
			try
			{
				using (var file = new StreamWriter(File.OpenWrite("state.ini")))
				{
					file.WriteLine(folderTextBox.Text);
					file.WriteLine(fileNameTextBox.Text);
					file.WriteLine(findStrTextBox.Text);
				}
			}
			catch { }
			
		}


		/// <summary>
		/// Мешает пользователю выполнить поиск по не существующей директории
		/// </summary>
		private void folderTextBox_TextChanged(object sender, EventArgs e)
		{
			if (!Directory.Exists(folderTextBox.Text))
			{
				findButton.Enabled = false;
			}
			else
				findButton.Enabled = true;
		}

		/// <summary>
		/// Выполняет подготовку и запускает поток поиска файлов
		/// </summary>
		private void findButton_Click(object sender, EventArgs e)
		{
			if (treeView1.Nodes.Count != 0)
				CallClear(findButton, null);
			timerData.Start();
			timer.Start();

			totalFiles = 0;
			findedFiles = 0;
			calculatedFiles = 0;

			secCnt = 0;
			workStr = findStrTextBox.Text;
			workDir = folderTextBox.Text;
			workPat = fileNameTextBox.Text.Trim();
			
			((Button)sender).Enabled = false;
			stopButton.Enabled = true;
			
			bytesUTF8 = Encoding.UTF8.GetBytes(workStr);
			bytesASCII = Encoding.ASCII.GetBytes(workStr);

			thread = new Thread(() =>
			{
				var dir = new DirectoryInfo(workDir);
				var tn = new TreeNode(workDir);
				evList.Add(new EventHandlerArgsAddMethod(() =>
				{
					treeView1.Nodes.Add(tn);
				}));
				SearchFile(dir, tn);
			});
			thread.Start();
		}


		/// <summary>
		/// Рекурсивно проходится по дереву катологов, выискивая подходящие файлы
		/// </summary>
		void SearchFile(DirectoryInfo dir, TreeNode tr)
		{
			try
			{
				foreach (var d in dir.GetDirectories())
				{
					if (!string.IsNullOrEmpty(d.Name.Trim()))
					{
						var ntr = new TreeNode(d.Name);
						lock (lockerEvent)
							evList.Add(new EventHandlerArgsAddMethod(() =>
							{
								tr.Nodes.Add(ntr);
							}));
						SearchFile(d, ntr);
					}
					else
						return;
				}
				foreach (var f in dir.GetFiles())
				{
					if (!string.IsNullOrEmpty(f.Name.Trim()))
					{
						lock (lockerNum)
							totalFiles++;
						var regex = new Regex(workPat);
						if (regex.Match(f.Name).Success && AnalizeFile(f))
						{
							lock (lockerNum)
								findedFiles++;
							lock (lockerEvent)
							{
								evList.Add(new EventHandlerArgsAddMethod(() =>
								{
									tr.ForeColor = Color.Red;
									var filetr = new TreeNode(f.Name);
									tr.Nodes.Add(filetr);
									treeView1.AfterSelect += (s, e) =>
									{
										if (((TreeView)s).SelectedNode == tr)
										{
											var y = rezPanel.Size.Width;
											var but = new Button() { Text = f.Name };
											but.Location = new Point((rezPanel.Controls.Count % (y / but.Size.Width)) * (but.Size.Width + 2),
																	(but.Height + 5) * (rezPanel.Controls.Count / (y / but.Size.Width)));
											but.Click += (se, ev) => Process.Start(f.FullName);
											rezPanel.Controls.Add(but);
											(new ToolTip()).SetToolTip(but, $"Полное имя файла: {f.FullName}\nРазмер: {f.Length} байт");
										}
										else if (((TreeView)s).SelectedNode == filetr)
											Process.Start(f.FullName);
									};
								}));
							}
						}
					}
					else
						return;
				}
					
			}
			catch (System.UnauthorizedAccessException)
			{
				lock (lockerStr)
					liveFile = "Не доступа к папке";
			}
			finally
			{
				lock (lockerEvent)
					evList.Add(new EventHandlerArgsAddMethod(() =>
					{
						if (tr.ForeColor != Color.Red && tr.Nodes.Count == 0 && tr.Parent != null)
							tr.Parent.Nodes.Remove(tr);
						else if (tr.Parent == null && tr.Nodes.Count == 0)
							treeView1.Nodes.Remove(tr);
					}));
			}
		}


		/// <summary>
		/// Выполняет анализ файла на содержание нужной подстроки
		/// </summary>
		/// <param name="fi">Анализируемый файл</param>
		/// <param name="ct"></param>
		/// <returns>Есть ли подстрока</returns>
		bool AnalizeFile(FileInfo fi)
		{
			var arrLenUTF8 = bytesUTF8.Length;
			var arrLenASCII = bytesASCII.Length;
			int mult = LENGTH / arrLenUTF8 > 2 ? LENGTH / arrLenUTF8 : 2;
			var bufLength = arrLenUTF8 * mult;
			lock (lockerStr)
				liveFile = fi.FullName;
			lock (lockerNum)
				calculatedFiles++;
			if (arrLenUTF8 == 0)
				return true;
			try
			{
				using (var stream = fi.OpenRead())
				{
					var buf = new byte[bufLength];
					int byteCounter = 0;
					while (byteCounter < stream.Length && (stream.Length >= arrLenASCII))
					{
						buf = buf.Skip(bufLength - arrLenUTF8).Concat(buf.Take(bufLength - arrLenUTF8)).ToArray();

						if (byteCounter > 0 && arrLenUTF8 > 1)
							stream.Seek(-arrLenUTF8, SeekOrigin.Current);
						byteCounter += stream.Read(buf, byteCounter > 0 ? arrLenUTF8 : 0, byteCounter > 0 ? bufLength - arrLenUTF8 : bufLength);

						if (byteCounter > arrLenUTF8)
							byteCounter -= arrLenUTF8 - 1;
						if (Encoding.UTF8.GetString(buf).Contains(workStr) || Encoding.Default.GetString(buf).Contains(workStr))
							return true;
					}
				}
			}
			catch (IOException)
			{
				lock (lockerStr)
					liveFile += "\nоткрыть не удалось";
			}
			return false;
		}

		private void timer_Tick(object sender, EventArgs e)
		{
			lock(lockerNum)
				totalFileLabel.Text = $"Найдено {findedFiles} Обработано {calculatedFiles} Всего {totalFiles} файлов";
			lock (lockerStr)
			{
				timiLabel.Text = $"Поиск длится: {TimeSpan.FromMilliseconds(++secCnt * 10.0):mm\\:ss\\:ff}\n {liveFile}";
			}
		}

		private void timerData_Tick(object sender, EventArgs e)
		{
			lock (lockerEvent)
			{
				foreach (var eva in evList)
				{
					this.CallTreeChange(this, eva);
				}
				evList.Clear();
			}
			if (thread != null && thread.ThreadState == System.Threading.ThreadState.Stopped)
			{

				timer.Stop();
				timerData.Stop();
				timiLabel.Text = $"Поиск длился: {TimeSpan.FromMilliseconds(++secCnt * 10.0):mm\\:ss\\:ff}\n";
				findButton.Enabled = true;
				stopButton.Enabled = false;
			}
		}
	}
}
