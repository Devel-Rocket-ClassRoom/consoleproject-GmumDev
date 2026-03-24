

using System.Drawing;
using System.Text;
using static FirstConsoleGame.Utility;

namespace FirstConsoleGame
{
	public class AlertRenderer
	{
		MyVector maxBufSize;
		char[,] buf;

		private AlertRenderBox[] alertBoxs = new AlertRenderBox[1];
		public AlertRenderBox alertGameoverBox;

		private Queue<AlertRenderBox.InputCallback> callbacks;
		private AlertRenderer()
		{
			maxBufSize = new MyVector(MAX_BUFFER_WIDTH / 4, MAX_BUFFER_HEIGHT / 4);
			buf = new char[maxBufSize.y, maxBufSize.x];
			callbacks = new Queue<AlertRenderBox.InputCallback>();

			var margin = new MyVector(MAX_BUFFER_WIDTH / 2 - maxBufSize.x / 2, MAX_BUFFER_HEIGHT / 2 - maxBufSize.y / 2);
			var size = maxBufSize;
			alertGameoverBox = new AlertRenderBox(margin, size, "Game Over");

			alertBoxs[0] = alertGameoverBox;
		}
		private static AlertRenderer instance;
		public static AlertRenderer GetInstance()
		{
			if (instance == null)
				instance = new AlertRenderer();

			return instance;
		}
	
		public void DrawAndSetCallbacks()
		{
			foreach(AlertRenderBox box in alertBoxs)
			{
				if(box.alerted)
				{
					box.alerted = false;
					box.Render(buf);

					for (int row = 0; row < box.Size.y; row++)
					{
						StringBuilder outbuf = new StringBuilder("");
						for (int col = 0; col < box.Size.x; col++)
						{
							outbuf.Append(buf[row, col]);
						}
						Console.SetCursorPosition(box.Margin.x, box.Margin.y + row);
						Console.WriteLine(outbuf);
					}

					callbacks.Enqueue(box.AlertGetCallbackByInput());
				}
			}
		}
		public void DoCallbacks()
		{
			while(callbacks.Count > 0)
			{
				callbacks.Dequeue()();
			}
		}
	}
}
