
using System.Text.Json;

namespace FirstConsoleGame
{
	public static class Utility
	{
		// ------------- Globals
		public const char EMPTY_SYMBOL = ' ';
		public const char FENCE_SYMBOL = '#';
		
		public const int MAX_BUFFER_HEIGHT = 50;
		public const int MAX_BUFFER_WIDTH = 200;
		
		public static Random rand = new Random((int)DateTime.Now.Ticks);

		private static HashSet<char> ableChars = new HashSet<char> { 'W', 'A', 'S', 'D', 'Q' };
		public static Dictionary<DirIndex, MyVector> dirToDeltaPos = new Dictionary<DirIndex, MyVector>
			{
				{DirIndex.Left, new MyVector(-1, 0)},
				{DirIndex.Right, new MyVector(1, 0)},
				{DirIndex.Up, new MyVector(0, -1)},
				{DirIndex.Down, new MyVector(0, 1)},
			};
		public static Dictionary<char, MyVector> charToDeltaPos = new Dictionary<char, MyVector>() {
				{ 'A', new MyVector(-1, 0) },
				{ 'D', new MyVector(1, 0) },
				{ 'W', new MyVector(0, -1) },
				{ 'S', new MyVector(0, 1) },
			};
		public static Dictionary<char, DirIndex> charToDirIndex = new Dictionary<char, DirIndex>()
			{
				{ 'A', DirIndex.Left},
				{ 'D', DirIndex.Right},
				{ 'W', DirIndex.Up},
				{ 'S', DirIndex.Down},
			};
		public static Dictionary<int, DirIndex> intToDirIndex = new Dictionary<int, DirIndex>()
			{
				{(int)DirIndex.Left, DirIndex.Left},
				{(int)DirIndex.Right, DirIndex.Right},
				{(int)DirIndex.Up, DirIndex.Up},
				{(int)DirIndex.Down, DirIndex.Down},
			};
		public static char InputSingleChar()
		{
			char c;
			string input_announcement = "[W / A / S / D / Q to quit]: ";
			Console.Write(input_announcement);
			do
			{
				c = Console.ReadKey(true).KeyChar;
				c = char.ToUpper(c);
			} while (ableChars.Contains(c) == false);

			return c;
		}
		public static void SaveData(object data, string path)
		{
			
			string result = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
			File.WriteAllText(Path.Combine("../../../Jsons/", path), result);
		}
		public static T LoadData<T>(string path)
		{
			string s = File.ReadAllText(Path.Combine("../../../Jsons/", path));
			var obj = JsonSerializer.Deserialize<T>(s);
			return obj;
		}
	}
}
