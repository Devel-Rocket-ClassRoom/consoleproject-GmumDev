using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstConsoleGame
{
	public class Monster : Entity
	{
		public override void Init(MyVector pos)
		{
			base.Init(pos);
			symbol = 'M';
		}
		public override string OnPlayerEnter()
		{
			Player player = MoveCapturing();
			player.AddMoney(1);
			var mapData = map.GetMapData();
			mapData[("monster_cnt")]--;
			if (mapData[("monster_cnt")] == 0)
			{
				return "You Killed Every Monster! Go Next Stage.";
			}
			return $"You moved to ({player.pos.x}, {player.pos.y})...Killed Monster! {mapData[("monster_cnt")]} monster left.";
		}
		public override EntityEnum GetEntityEnum()
		{
			return EntityEnum.Monster;
		}
	}
}
