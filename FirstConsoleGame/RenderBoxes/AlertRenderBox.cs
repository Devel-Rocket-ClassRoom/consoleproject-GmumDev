
namespace FirstConsoleGame
{
	// usage: new -> Init -> SetCallback -> ... -> LocalRender
	public class AlertRenderBox : RenderBox
	{

		public delegate void InputCallback();
		private Dictionary<char, (string, InputCallback)> inputCallbackDict;
		protected string title;

		public bool alerted;
		public AlertRenderBox(MyVector margin, MyVector size, string title) : base(margin, size)
		{
			this.title = title;
			DrawBorder();
		}
		public override void DrawBorder()
		{
			DrawBorder($" [{title}]");
		}
		public void SetCallback(char input_char, string msg, InputCallback callback)
		{
			if (inputCallbackDict.Keys.Contains(input_char) == false)
				inputCallbackDict.Add(char.ToUpper(input_char), (msg, callback));
			else
				inputCallbackDict[char.ToUpper(input_char)] = (msg, callback);
		}
		public override void Init()
		{
			inputCallbackDict = new Dictionary<char, (string, InputCallback)>();
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
		protected virtual (int, int, int) GetElemAlignment()
		{
			return (10, 3, 3);
		}
		public void LocalRender()
		{
			int cnt = inputCallbackDict.Count;

			(int elemWidth, int elemHeight, int elemPerWidth) = GetElemAlignment();

			int paddingx = (size.x / elemPerWidth) / 2 - elemWidth/2;
			int paddingy = 2;
			char[] callbackChars = inputCallbackDict.Keys.ToArray();
			for (int i = 0; i < cnt; i++)
			{
				string msg = inputCallbackDict[callbackChars[i]].Item1;
				int min_x_offset = Math.Min(elemWidth / 2, msg.Length / 2);
				int x = paddingx + (size.x / elemPerWidth) * (i % elemPerWidth) + elemWidth/2 - min_x_offset;
				int y = paddingy + (i / elemPerWidth) * elemHeight;

				int it = 0; // msg iterator


				// draw symbol
				buf[y, x + min_x_offset] = callbackChars[i];

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
