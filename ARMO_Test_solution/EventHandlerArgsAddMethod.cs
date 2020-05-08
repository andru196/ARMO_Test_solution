using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARMO_Test_solution
{
	class EventHandlerArgsAddMethod : EventArgs

	{
		public EventHandlerArgsAddMethod (Action a)
		{
			ActionToRightTHread = a;
		}
		public Action ActionToRightTHread;
	}
}
