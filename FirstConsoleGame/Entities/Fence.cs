
using static FirstConsoleGame.Utility;
namespace FirstConsoleGame
{
	public class Fence : Entity
	{
		public override void Init(MyVector pos)
		{
			base.Init(pos);
			symbol = FENCE_SYMBOL;
		}
		public override string OnPlayerEnter()
		{
			return "Fail to move: conflicts wall";
		}
	}
}
