using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstConsoleGame
{
	public class BigSnake_Head : Snake_Head
	{
		public override void Init(MyVector pos)
		{
			base.Init(pos);

			AddTail();
			AddTail();
			AddTail();
			AddTail();
		}
	}
}
