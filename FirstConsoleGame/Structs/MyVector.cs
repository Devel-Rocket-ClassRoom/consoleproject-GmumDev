using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstConsoleGame
{
	public struct MyVector
	{
		public int x;
		public int y;
		public MyVector(int x, int y)
		{
			this.x = x;
			this.y = y;
		}
		public (int, int) GetTuple()
		{
			return (x, y);
		}
		public bool IsOutOfSquare(MyVector v)
		{
			return v.x < 0 || v.x > this.x - 1 || v.y < 0 || v.y > this.y - 1;
		}
		public static MyVector operator +(MyVector v1, MyVector v2)
		{
			return new MyVector(v1.x + v2.x, v1.y + v2.y);
		}
	}
}
