using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstConsoleGame
{
	public class HealingPotion : Entity
	{
		public override void Init(MyVector pos)
		{
			base.Init(pos);
			symbol = '8';
		}
		public override string OnPlayerEnter()
		{
			Player player = MoveCapturing();

			int prevHp = player.Hp;
			player.TakeDamage(-1);
			int newHp = player.Hp;

			return $"You moved to ({player.pos.x}, {player.pos.y})...Got healing potion! Hp {prevHp} -> {newHp}";
		}
		public override EntityEnum GetEntityEnum()
		{
			return EntityEnum.HealingPotion;
		}
	}
}
