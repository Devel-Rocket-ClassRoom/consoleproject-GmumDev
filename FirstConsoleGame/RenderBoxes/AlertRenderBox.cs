
namespace FirstConsoleGame
{
	internal class AlertRenderBox : RenderBox
	{
		public delegate void InputCallback();

		private Dictionary<char, InputCallback> inputCallbackDict;
		private string msg;
		private string title;
		public AlertRenderBox(MyVector margin, MyVector size, string title) : base(margin, size)
		{
			inputCallbackDict = new Dictionary<char, InputCallback>();
			this.title = title;
			Init();
		}
		public void SetInput(char input_char, InputCallback callback)
		{
			inputCallbackDict.Add(input_char, callback);
		}
		public override void Init()
		{
			DrawBorder($" [{title}]");
		}
		public override void Render(char[,] parentbuf)
		{
			int cnt = inputCallbackDict.Count;

			base.Render(parentbuf);
		}

	}
}
