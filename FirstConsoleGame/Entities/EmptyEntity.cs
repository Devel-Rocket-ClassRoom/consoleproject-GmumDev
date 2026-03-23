
using static FirstConsoleGame.Utility;
namespace FirstConsoleGame
{
	public class EmptyEntity : Entity
	{
		public override void Init(MyVector pos)
		{
			base.Init(pos);
			symbol = EMPTY_SYMBOL;
		}
		public override string OnPlayerEnter()
		{
			Player player = MoveCapturing();
			return "You moved to (" + player.pos.x + ", " + player.pos.y + ")...";
		}
	}
}
