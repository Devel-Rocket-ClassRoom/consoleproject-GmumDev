using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstConsoleGame
{
	public class PlayerRenderBox : RenderBox
	{
		public PlayerRenderBox(MyVector size) : base(size) { }
		public PlayerRenderBox(MyVector margin, MyVector size) : base(margin, size) { }
		public void Update(int maxhp, int hp)
		{
			DrawLife(maxhp, hp);
		}
		private void DrawSingleLife(int x, int y, bool isOn)
		{
			char[,] onLifeChar =
			{
					{'┏','┓'},
					{'┗','┛'},
				};
			char[,] offLifeChar =
			{
					{'┌','┐'},
					{'└','┘'},
				};
			if (isOn)
				for (int i = 0; i < 2; i++)
					for (int j = 0; j < 2; j++)
						buf[y + i, x + j] = onLifeChar[i, j];
			else
				for (int i = 0; i < 2; i++)
					for (int j = 0; j < 2; j++)
						buf[y + i, x + j] = offLifeChar[i, j];
		}
		private void DrawLife(int maxhp, int hp)
		{
			int lifePerLine = 4;
			int lifeLine = maxhp / lifePerLine + (maxhp % lifePerLine == 0 ? 0 : 1);
			int lifeImgSize = 2;
			HashSet<MyVector> lifePivots = new HashSet<MyVector>();
			for (int i = 0; i < maxhp; i++)
			{
				lifePivots.Add(new MyVector(
					(size.x / lifePerLine) * (i % lifePerLine) + (size.x / lifePerLine) / 2 - lifeImgSize / 2,
					((size.y / lifeLine) * (i / lifePerLine) + (size.y / lifeLine) / 2 - lifeImgSize / 2) / 2)
				);
			}

			for (int r = 1; r < size.y; r++)
			{
				for (int c = 1; c < size.x; c++)
				{
					if (lifePivots.Contains(new MyVector(c, r)))
					{
						if (hp-- > 0)
							DrawSingleLife(c, r, true);
						else
							DrawSingleLife(c, r, false);
					}
				}
			}
		}
		public override void DrawBorder()
		{
			DrawBorder(" [You]");
		}
		public override void Init()
		{
			DrawBorder();
		}
	}
}
