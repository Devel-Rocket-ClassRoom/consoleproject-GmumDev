using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstConsoleGame
{
	public class MapRenderBox : RenderBox
	{
		// Todo: 드로우 명령을 RenderBox와 호환 가능하도록 리팩토링. 현재 상위 Renderer에서 예외처리되고있음
		public MyVector padding;
		private MyVector curMapSize;
		private int curStageNum = 1;

		public MapRenderBox(MyVector size) : base(size) { }
		public MapRenderBox(MyVector margin, MyVector size) : base(margin, size) { }
		public override void Init()
		{
			
			DrawBorder();
		}
		public override void DrawBorder()
		{
			DrawBorder($" [Stage {curStageNum}]");
		}
		public void Update(DungeonGame_Map map, int stageNum)
		{
			if (curStageNum != stageNum)
				Init();
			curStageNum = stageNum;
			padding = new MyVector((size.x - map.size.x) / 2, (size.y - map.size.y) / 2);
			curMapSize = map.size;

			char[,] tmpbuf = new char[map.size.y, map.size.x];
			for (int r = 0; r < map.size.y; r++)
			{
				for (int c = 0; c < map.size.x; c++)
				{
					map.GetEntity(c, r).Render(tmpbuf);

					buf[r + padding.y, c + padding.x] = tmpbuf[r, c];
				}
			}
		}
	}
}
