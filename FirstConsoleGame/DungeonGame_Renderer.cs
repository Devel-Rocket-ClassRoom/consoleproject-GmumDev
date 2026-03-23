
using System.Text;

using static FirstConsoleGame.Utility;

namespace FirstConsoleGame
{
	public class DungeonGame_Renderer
	{
		char[,] buf;

		private enum RenderIndex
		{
			Map,
			Balloon,
			Minimap,
			Player,
			Input,
		};
		private RenderBox[] renderBoxs = new RenderBox[Enum.GetNames(typeof(RenderIndex)).Length];

		private MapRenderBox mapbox;
		private MsgRenderBox msgbox;
		private MinimapRenderBox minimapBox;
		private PlayerRenderBox playerbox;
		private InputBox inputBox;
		


		// ----- Singleton + Constructor
		private static DungeonGame_Renderer instance;
		private DungeonGame_Renderer(MyVector maxMapSize)
		{
			buf = new char[MAX_BUFFER_HEIGHT, MAX_BUFFER_WIDTH];

			MyVector margin;
			MyVector size;

			margin = new MyVector(2, 2);
			size = maxMapSize + new MyVector(2, 2);
			mapbox = new MapRenderBox(margin, size);
			renderBoxs[(int)RenderIndex.Map] = mapbox;

			margin = new MyVector(mapbox.Margin.x + mapbox.Size.x, mapbox.Margin.y);
			size = new MyVector((MAX_BUFFER_WIDTH - margin.x) / 4, 15);
			minimapBox = new MinimapRenderBox(margin, size);
			renderBoxs[(int)RenderIndex.Minimap] = minimapBox;

			margin = new MyVector(minimapBox.Margin.x + minimapBox.Size.x, minimapBox.Margin.y);
			size = new MyVector(MAX_BUFFER_WIDTH - margin.x, minimapBox.Size.y);
			msgbox = new MsgRenderBox(margin, size);
			renderBoxs[(int)RenderIndex.Balloon] = msgbox;

			margin = new MyVector(minimapBox.Margin.x, minimapBox.Margin.y + minimapBox.Size.y);
			size = new MyVector(minimapBox.Size.x, minimapBox.Size.y);
			playerbox = new PlayerRenderBox(margin, size);
			renderBoxs[(int)RenderIndex.Player] = playerbox;


			margin = new MyVector(minimapBox.Margin.x, playerbox.Margin.y + playerbox.Size.y);
			size = new MyVector(minimapBox.Size.x, MAX_BUFFER_HEIGHT - margin.y - 1);
			inputBox = new InputBox(margin, size);
			renderBoxs[(int)RenderIndex.Input] = inputBox;

		}
		public static DungeonGame_Renderer GetInstance(MyVector maxMapSize)
		{
			if (instance == null)
			{
				instance = new DungeonGame_Renderer(maxMapSize);
				for (int i = 0; i < MAX_BUFFER_HEIGHT; i++)
				{
					for (int j = 0; j < MAX_BUFFER_WIDTH; j++)
					{
						instance.buf[i, j] = EMPTY_SYMBOL;
					}
				}
			}

			return instance;
		}

		// ----- 
		public void Update(DungeonGame_Map map, int stage_num, string msg, int maxhp, int hp)
		{
			mapbox.Update(map, stage_num);
			msgbox.Update(msg);
			minimapBox.Update(map);
			playerbox.Update(maxhp, hp);
		}
		public void Draw()
		{

			foreach (RenderBox box in renderBoxs)
				box.Render(buf);

			StringBuilder outbuf = new StringBuilder("");
			for (int i = 0; i < MAX_BUFFER_HEIGHT; i++)
			{
				for (int j = 0; j < MAX_BUFFER_WIDTH; j++)
				{
					outbuf.Append(buf[i, j]);
					buf[i, j] = EMPTY_SYMBOL;
				}
				outbuf.Append('\n');   // for windows
			}
			Console.SetCursorPosition(0, 0);
			Console.WriteLine(outbuf);
		}
		public char[,] getBuf() { return buf; }
		public void UpdateNewStage()
		{
			foreach (var box in renderBoxs)
				box.Init();
		}
		
	}
}
