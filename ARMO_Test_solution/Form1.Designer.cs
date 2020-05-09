namespace ARMO_Test_solution
{
	partial class Form1
	{
		/// <summary>
		/// Обязательная переменная конструктора.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Освободить все используемые ресурсы.
		/// </summary>
		/// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Код, автоматически созданный конструктором форм Windows

		/// <summary>
		/// Требуемый метод для поддержки конструктора — не изменяйте 
		/// содержимое этого метода с помощью редактора кода.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.panel1 = new System.Windows.Forms.Panel();
			this.timiLabel = new System.Windows.Forms.Label();
			this.findButton = new System.Windows.Forms.Button();
			this.stopButton = new System.Windows.Forms.Button();
			this.rezPanel = new System.Windows.Forms.Panel();
			this.folderTextBox = new System.Windows.Forms.TextBox();
			this.fileNameTextBox = new System.Windows.Forms.TextBox();
			this.findStrTextBox = new System.Windows.Forms.TextBox();
			this.opendirButton = new System.Windows.Forms.Button();
			this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
			this.timer = new System.Windows.Forms.Timer(this.components);
			this.timerData = new System.Windows.Forms.Timer(this.components);
			this.totalFileLabel = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// treeView1
			// 
			this.treeView1.Location = new System.Drawing.Point(13, 13);
			this.treeView1.Name = "treeView1";
			this.treeView1.Size = new System.Drawing.Size(266, 425);
			this.treeView1.TabIndex = 0;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.timiLabel);
			this.panel1.Location = new System.Drawing.Point(285, 410);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(503, 28);
			this.panel1.TabIndex = 1;
			// 
			// timiLabel
			// 
			this.timiLabel.AutoSize = true;
			this.timiLabel.Location = new System.Drawing.Point(3, 0);
			this.timiLabel.Name = "timiLabel";
			this.timiLabel.Size = new System.Drawing.Size(0, 13);
			this.timiLabel.TabIndex = 0;
			// 
			// findButton
			// 
			this.findButton.Location = new System.Drawing.Point(687, 39);
			this.findButton.Name = "findButton";
			this.findButton.Size = new System.Drawing.Size(109, 20);
			this.findButton.TabIndex = 2;
			this.findButton.Text = "Найти";
			this.findButton.UseVisualStyleBackColor = true;
			this.findButton.EnabledChanged += new System.EventHandler(this.findButton_EnabledChanged);
			this.findButton.Click += new System.EventHandler(this.findButton_Click);
			// 
			// stopButton
			// 
			this.stopButton.Enabled = false;
			this.stopButton.Location = new System.Drawing.Point(687, 65);
			this.stopButton.Name = "stopButton";
			this.stopButton.Size = new System.Drawing.Size(109, 20);
			this.stopButton.TabIndex = 3;
			this.stopButton.Text = "Остановить";
			this.stopButton.UseVisualStyleBackColor = true;
			// 
			// rezPanel
			// 
			this.rezPanel.Location = new System.Drawing.Point(286, 142);
			this.rezPanel.Name = "rezPanel";
			this.rezPanel.Size = new System.Drawing.Size(502, 262);
			this.rezPanel.TabIndex = 6;
			// 
			// folderTextBox
			// 
			this.folderTextBox.Location = new System.Drawing.Point(285, 13);
			this.folderTextBox.Name = "folderTextBox";
			this.folderTextBox.Size = new System.Drawing.Size(396, 20);
			this.folderTextBox.TabIndex = 8;
			this.folderTextBox.TextChanged += new System.EventHandler(this.folderTextBox_TextChanged);
			// 
			// fileNameTextBox
			// 
			this.fileNameTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.fileNameTextBox.ForeColor = System.Drawing.SystemColors.ScrollBar;
			this.fileNameTextBox.Location = new System.Drawing.Point(285, 39);
			this.fileNameTextBox.Name = "fileNameTextBox";
			this.fileNameTextBox.Size = new System.Drawing.Size(396, 18);
			this.fileNameTextBox.TabIndex = 9;
			this.fileNameTextBox.TextChanged += new System.EventHandler(this.fileNameTextBox_TextChanged);
			// 
			// findStrTextBox
			// 
			this.findStrTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.findStrTextBox.ForeColor = System.Drawing.SystemColors.ScrollBar;
			this.findStrTextBox.Location = new System.Drawing.Point(285, 65);
			this.findStrTextBox.Name = "findStrTextBox";
			this.findStrTextBox.Size = new System.Drawing.Size(396, 18);
			this.findStrTextBox.TabIndex = 10;
			// 
			// opendirButton
			// 
			this.opendirButton.Location = new System.Drawing.Point(687, 13);
			this.opendirButton.Name = "opendirButton";
			this.opendirButton.Size = new System.Drawing.Size(109, 20);
			this.opendirButton.TabIndex = 11;
			this.opendirButton.Text = "Выбрать папку";
			this.opendirButton.UseVisualStyleBackColor = true;
			this.opendirButton.Click += new System.EventHandler(this.opendirButton_Click);
			// 
			// folderBrowserDialog
			// 
			this.folderBrowserDialog.ShowNewFolderButton = false;
			// 
			// totalFileLabel
			// 
			this.totalFileLabel.AutoSize = true;
			this.totalFileLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.totalFileLabel.Location = new System.Drawing.Point(285, 86);
			this.totalFileLabel.Name = "totalFileLabel";
			this.totalFileLabel.Size = new System.Drawing.Size(0, 15);
			this.totalFileLabel.TabIndex = 12;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.25F);
			this.label2.Location = new System.Drawing.Point(282, 117);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(142, 22);
			this.label2.TabIndex = 13;
			this.label2.Text = "Файлы в папке:";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(799, 450);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.totalFileLabel);
			this.Controls.Add(this.opendirButton);
			this.Controls.Add(this.findStrTextBox);
			this.Controls.Add(this.fileNameTextBox);
			this.Controls.Add(this.folderTextBox);
			this.Controls.Add(this.rezPanel);
			this.Controls.Add(this.stopButton);
			this.Controls.Add(this.findButton);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.treeView1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TreeView treeView1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button findButton;
		private System.Windows.Forms.Button stopButton;
		private System.Windows.Forms.Panel rezPanel;
		private System.Windows.Forms.TextBox folderTextBox;
		private System.Windows.Forms.TextBox fileNameTextBox;
		private System.Windows.Forms.TextBox findStrTextBox;
		private System.Windows.Forms.Button opendirButton;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
		private System.Windows.Forms.Timer timer;
		private System.Windows.Forms.Label timiLabel;
		private System.Windows.Forms.Timer timerData;
		private System.Windows.Forms.Label totalFileLabel;
		private System.Windows.Forms.Label label2;
	}
}

