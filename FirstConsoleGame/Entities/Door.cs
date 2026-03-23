using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FirstConsoleGame.Program;

namespace FirstConsoleGame
{

	public class Door : Entity
	{
		private DungeonGame_Map targetMap;
		public override void Init(MyVector pos)
		{
			base.Init(pos);
			symbol = 'D';
		}
		public void ConnectTo(DungeonGame_Map map)
		{
			this.targetMap = map;
		}
		public DungeonGame_Map GetTargetMap()
		{
			return targetMap;
		}
		public override string OnPlayerEnter()
		{
			return "Door Locked!";
		}
	}
}
