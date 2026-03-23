
namespace FirstConsoleGame
{
	public class AlertRenderBox : RenderBox
	{
		public delegate void InputCallback();
		private Dictionary<char, (string, InputCallback)> inputCallbackDict;
		private string title;

		public bool alerted;
		public AlertRenderBox(MyVector margin, MyVector size, string title) : base(margin, size)
		{
			this.title = title;
			Init();
		}
		public void SetCallback(char input_char, string msg, InputCallback callback)
		{
			if(inputCallbackDict.Keys.Contains(input_char) == false)
				inputCallbackDict.Add(char.ToUpper(input_char), (msg, callback));
		}
		public override void Init()
		{
			inputCallbackDict = new Dictionary<char, (string, InputCallback)>();
			DrawBorder($" [{title}]");
			alerted = false;
		}

		public void Alert()
		{
			alerted = true;
		}
		public InputCallback GetCallbackByInput()
		{
			char c;
			do
			{
				c = Console.ReadKey(true).KeyChar;
				c = char.ToUpper(c);
			} while (inputCallbackDict.Keys.Contains(c) == false);

			return inputCallbackDict[c].Item2;
		}
		public void LocalRender()
		{
			int cnt = inputCallbackDict.Count;

			int elemPerWidth = 2;
			int elemWidth = 10;
			int elemHeight = 3;

			int paddingx = (size.x / elemPerWidth) / 2 - elemWidth/2;
			int paddingy = 2;
			char[] callbackChars = inputCallbackDict.Keys.ToArray();
			for (int i = 0; i < cnt; i++)
			{
				string msg = inputCallbackDict[callbackChars[i]].Item1;
				int x = paddingx + (size.x / elemPerWidth) * (i % elemPerWidth) + elemWidth/2 - msg.Length/2;
				int y = paddingy + (i / elemPerWidth) * elemHeight;

				int it = 0; // msg iterator


				// draw key(input char)
				buf[y, x + msg.Length/2 - 1] = callbackChars[i];

				// draw messages
				for (int local_y = y + 1; local_y < y + elemHeight; local_y++)
				{
					for (int local_x = x; local_x < x + elemWidth; local_x++)
					{
						if (size.IsOutOfSquare(new MyVector(local_x, local_y))) continue;

						if (it < msg.Length)
							buf[local_y, local_x] = msg[it++];
					}
				}
			}
		}
		public override void Render(char[,] parentbuf)
		{
			for (int r = 0; r < size.y; r++)
			{
				for (int c = 0; c < size.x; c++)
				{
					parentbuf[r, c] = buf[r, c];
				}
			}
		}

	}
}
