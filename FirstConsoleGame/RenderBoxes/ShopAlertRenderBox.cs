using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstConsoleGame
{
	public class ShopAlertRenderBox : AlertRenderBox
	{
		public const int elemWidth = 16;
		public ShopAlertRenderBox(MyVector margin, MyVector size, string title) : base(margin, size, title)
		{

		}
		protected override (int, int, int) GetElemAlignment()
		{
			return (elemWidth, 4, 3);
		}
		public void Redraw()
		{
			DrawBorder($"[{title}]");
			LocalRender();
		}
	}
}
