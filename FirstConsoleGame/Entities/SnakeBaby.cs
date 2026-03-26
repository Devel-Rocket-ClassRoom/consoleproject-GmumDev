
using static FirstConsoleGame.Utility;
namespace FirstConsoleGame
{
	public class SnakeBaby : Entity
	{
		private int turnToGrow;
		public override void Init(MyVector pos)
		{
			base.Init(pos);
			symbol = 'M';
			turnToGrow = rand.Next() % 20 + 20;
		}
		public override void Update()
		{
			base.Update();
			if (turnToGrow < 5)
			{
				symbol = 'H';
			}
			if (--turnToGrow <= 0)
			{
				map.SetNewEntity<Snake_Head>(pos);
				map.GetEntity(pos.x, pos.y).IsUpdatedOnCurFrame = true;
				return;
			}
		}
		public override string OnPlayerEnter()
		{
			Player player = MoveCapturing();
			return $"You moved to ({player.pos.x}, {player.pos.y})...Eliminated Snake Egg! It was about to grow up after {turnToGrow} Turn(s).";
		}
		public override EntityEnum GetEntityEnum()
		{
			return EntityEnum.SnakeBaby;
		}
	}
}
