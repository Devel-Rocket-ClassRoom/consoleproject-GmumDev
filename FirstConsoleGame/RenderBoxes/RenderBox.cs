using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstConsoleGame
{
	public abstract class RenderBox : IRenderable
	{
		public MyVector Size { get => size; }
		protected MyVector size { get; }

		public MyVector Margin { get => margin; }

		protected MyVector margin;

		public char[,] buf;

		protected RenderBox(MyVector size)
		{
			this.buf = new char[size.y, size.x];
			this.margin = new MyVector(0, 0);
			this.size = size;
			Init();
		}
		protected RenderBox(MyVector margin, MyVector size)
		{
			this.buf = new char[size.y, size.x];
			this.margin = margin;
			this.size = size;
			Init();
		}
		public abstract void Init();
		public virtual void Render(char[,] parentbuf)
		{
			for (int r = 0; r < size.y; r++)
			{
				for (int c = 0; c < size.x; c++)
				{
					int globalx = c;
					int globaly = r;
					parentbuf[r + margin.y, c + margin.x] = buf[r, c];
				}
			}

			for (int r = 1; r < size.y - 1; r++)
			{
				for (int c = 1; c < size.x - 1; c++)
				{
					buf[r, c] = ' ';
				}
			}
		}
		public abstract void DrawBorder();
		protected void DrawBorder(string str)
		{
			for (int i = 0; i < size.y; i++)
			{
				for (int j = 0; j < size.x; j++)
				{
					if (i == 0 || i == size.y - 1)
						buf[i, j] = '─';
					else if (j == 0 || j == size.x - 1)
						buf[i, j] = '│';
					else
						buf[i, j] = ' ';
				}
			}
			buf[0, 0] = '┌';
			buf[size.y - 1, 0] = '└';
			buf[0, size.x - 1] = '┐';
			buf[size.y - 1, size.x - 1] = '┘';

			MyVector pos = new MyVector(size.x / 2 - str.Length / 2, 0);
			for (int i = 0; i < str.Length; i++)
				buf[pos.y, pos.x + i] = str[i];
		}
	}
}
