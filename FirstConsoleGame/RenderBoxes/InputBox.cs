using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstConsoleGame
{
	public class InputBox : RenderBox
	{
		public InputBox(MyVector size) : base(size) { }
		public InputBox(MyVector margin, MyVector size) : base(margin, size) { }
		public override void Init()
		{
			DrawBorder($" [Input]");
		}
	}
}
