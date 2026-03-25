

using static FirstConsoleGame.Utility;
namespace FirstConsoleGame
{
	public class MinimapRenderBox : RenderBox
	{
		private MyVector curMapPos;
		private List<MyVector> mapPosList;
		public MinimapRenderBox(MyVector size) : base(size) { }
		public MinimapRenderBox(MyVector margin, MyVector size) : base(margin, size) { }

		public override void Init()
		{
			var factory = DungeonGame_StageFactory.GetInstance();
			mapPosList = new List<MyVector>();
			for (int r = 0; r < DungeonGame_StageFactory.GRID_HEIGHT; r++)
			{
				for (int c = 0; c < DungeonGame_StageFactory.GRID_WIDTH; c++)
				{
					var pos = new MyVector(c, r);
					var map = factory.GetMap(pos);
					if (map != null)
					{
						mapPosList.Add(pos);
					}
				}
			}
			DrawBorder();
		}
		public override void DrawBorder()
		{
			DrawBorder(" [Mini Map]");
		}
		public void Update(DungeonGame_Map curmap)
		{
			curMapPos = curmap.GridPos;
		}
		public override void Render(char[,] parentbuf)
		{
			// clear
			for (int local_y = 1; local_y < size.y - 1; local_y++)
			{
				for (int local_x = 1; local_x < size.x - 1; local_x++)
				{
					buf[local_y, local_x] = EMPTY_SYMBOL;
				}
			}

			MyVector center = new MyVector(size.x / 2, size.y / 2);
			Square drawboxSquare = new Square(1, 1, size.x - 2, size.y - 2);
			char[,] localMapBorderChar =
			{
					{'┌','─','┐'},
					{'│',' ','│'},
					{'└','─','┘'},
				};

			int zoom = 3;
			foreach (var pos in mapPosList)
			{
				MyVector zoomed = new MyVector((pos.x - curMapPos.x) * zoom, (pos.y - curMapPos.y) * zoom);
				MyVector localPos = center + zoomed;
				for (int i = 0; i < zoom; i++)
				{
					for (int j = 0; j < zoom; j++)
					{
						MyVector tmp = localPos + new MyVector(j - zoom / 2, i - zoom / 2);

						char _c = localMapBorderChar[i, j]; // works properly only where zoom == 3;

						if (drawboxSquare.IsOutOfSquare(tmp) == false)
							buf[tmp.y, tmp.x] = _c;
					}
				}
			}
			buf[size.y / 2, size.x / 2] = '┼';


			// print
			for (int local_y = 0; local_y < size.y; local_y++)
			{
				for (int local_x = 0; local_x < size.x; local_x++)
				{
					int global_x = local_x + margin.x;
					int global_y = local_y + margin.y;
					parentbuf[global_y, global_x] = buf[local_y, local_x];
				}
			}


		}
	}
}
