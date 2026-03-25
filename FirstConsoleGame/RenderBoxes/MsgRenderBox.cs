
using static FirstConsoleGame.Utility;
namespace FirstConsoleGame
{
	public class MsgRenderBox : RenderBox
	{
		public string msg = "";
		public MsgRenderBox(MyVector size) : base(size) { }
		public MsgRenderBox(MyVector margin, MyVector size) : base(margin, size) { }

		public override void Init()
		{
			DrawBorder();
		}
		public void Update(string msg)
		{
			this.msg = msg;
		}
		public override void DrawBorder()
		{
			DrawBorder(" [Text]");
		}
		public override void Render(char[,] parentbuf)
		{
			int padding_x = 3;  //padding include outliner
			int padding_y = 2;

			int msg_curx = padding_x;
			int msg_cury = padding_y;

			int msg_ex = size.x - 2 - padding_x;
			int msg_ey = size.y - 2 - padding_y;

			int it = 0; // msg iterator

			// clear
			for (int local_y = 1; local_y < size.y - 1; local_y++)
			{
				for (int local_x = 1; local_x < size.x - 1; local_x++)
				{
					buf[local_y, local_x] = EMPTY_SYMBOL;
				}
			}

			// print msg
			for (int local_y = 0; local_y < size.y; local_y++)
			{
				for (int local_x = 0; local_x < size.x; local_x++)
				{
					int global_x = local_x + margin.x;
					int global_y = local_y + margin.y;
					if (local_x == msg_curx && local_y == msg_cury && it < msg.Length)
					{
						buf[local_y, local_x] = msg[it++];
						msg_curx++;
						if (msg_curx > msg_ex)
						{
							msg_curx = padding_x;
							msg_cury++;
						}
					}
					parentbuf[global_y, global_x] = buf[local_y, local_x];
				}
			}
		}

	}
}
