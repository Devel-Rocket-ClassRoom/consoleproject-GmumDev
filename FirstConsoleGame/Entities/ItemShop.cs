using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstConsoleGame
{
	public class ItemShop : Entity
	{
		private AlertRenderBox shopBox;
		public override void Init(MyVector pos)
		{
			base.Init(pos);
			symbol = 'S';


		}
		public override string OnPlayerEnter()
		{
			var mapdata = map.GetMapData();
			MyVector playerPos = new MyVector(mapdata["player_x"], mapdata["player_y"]);
			Player player = (Player)map.GetEntity(playerPos.x, playerPos.y);



			throw new NotImplementedException();
		}
	}
}
