

using System.Drawing;
using System.Text;
using static FirstConsoleGame.Utility;

namespace FirstConsoleGame
{
	public class AlertRenderer
	{
		MyVector maxBufSize;
		char[,] buf;

		// all boxes
		private AlertRenderBox[] boxes;
		
		// que to do alert, do callbacks. 
		private Queue<AlertRenderBox> alertedBox;
		private Queue<AlertRenderBox.InputCallback> callbacks;

		public AlertRenderBox alertGameoverBox;
		public AlertRenderBox alertDonateBox;
		public AlertRenderBox alertShopBox;

		private AlertRenderer()
		{
			maxBufSize = new MyVector(MAX_BUFFER_WIDTH / 4, MAX_BUFFER_HEIGHT / 4);
			buf = new char[maxBufSize.y, maxBufSize.x];
			callbacks = new Queue<AlertRenderBox.InputCallback>();

			var margin = new MyVector(MAX_BUFFER_WIDTH / 2 - maxBufSize.x / 2, MAX_BUFFER_HEIGHT / 2 - maxBufSize.y / 2);
			var size = maxBufSize;

			alertGameoverBox = new AlertRenderBox(margin, size, "Game Over");
			alertDonateBox = new AlertRenderBox(margin, size, "Donate");
			alertShopBox = new AlertRenderBox(margin, size, "Shop");

			boxes = new AlertRenderBox[3];
			boxes[0] = alertGameoverBox;
			boxes[1] = alertDonateBox;
			boxes[2] = alertShopBox;

			alertedBox = new Queue<AlertRenderBox>();
		}
		private static AlertRenderer instance;
		public static AlertRenderer GetInstance()
		{
			if (instance == null)
				instance = new AlertRenderer();

			return instance;
		}
	
		public int UpdateAlertedCnt()
		{
			foreach(AlertRenderBox box in boxes)
			{
				if (box.alerted) alertedBox.Enqueue(box);
			}
			return alertedBox.Count;
		}
		public void DrawAndSetCallbacks()
		{
			while(alertedBox.Count > 0)
			{
				var box = alertedBox.Dequeue();

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

				callbacks.Enqueue(box.GetCallbackByInput());
			}
		}
		public void DoCallbacks()
		{
			while (callbacks.Count > 0)
			{
				callbacks.Dequeue()();
			}
		}
	}
}
