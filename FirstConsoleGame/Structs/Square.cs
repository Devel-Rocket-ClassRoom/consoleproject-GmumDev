using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstConsoleGame
{
	public struct Square
	{
		public MyVector pos;
		public MyVector size;
		public Square(int x, int y, int w, int h)
		{
			pos = new MyVector(x, y);
			size.x = w;
			size.y = h;
		}
		public bool IsOutOfSquare(MyVector v)
		{
			return v.x < pos.x || v.x > pos.x + size.x - 1 || v.y < pos.y || v.y > pos.y + size.y - 1;
		}
	}
}
