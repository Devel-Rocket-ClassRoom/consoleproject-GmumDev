
using static FirstConsoleGame.Utility;

namespace FirstConsoleGame
{
	internal class Program
	{
		// ------------- Main
		static void Main(string[] args)
		{
			Console.CursorVisible = false;
			Console.SetWindowSize(MAX_BUFFER_WIDTH + 2, MAX_BUFFER_HEIGHT + 2);
			DungeonGame game = DungeonGame.GetInstance();
			game.Play();
			return;
		}
	}
}
