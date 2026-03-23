using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstConsoleGame
{
	public class Snake_Tail : Entity
	{
		public override void Init(MyVector pos)
		{
			base.Init(pos);
			symbol = 'O';
		}
		public override string OnPlayerEnter()
		{
			var player = (Player)map.GetEntity(map.GetMapData()["player_x"], map.GetMapData()["player_y"]);
			player.TakeDamage(1);

			return $"You Tried to dive Strong Monster! Life : {player.Hp}. Never Do that!!!";
		}
	}
}
