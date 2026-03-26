using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstConsoleGame
{
	public class MapClearBicon : Entity
	{
		// bicon to clear map
		public override void Init(MyVector pos)
		{
			base.Init(pos);
			symbol = '*';
		}
		public override string OnPlayerEnter()
		{
			_ = MoveCapturing();
			map.SetClear(true);
			return "Map Clear, door Opened!!!";
		}
		public override EntityEnum GetEntityEnum()
		{
			return EntityEnum.MapClearBicon;
		}
	}
}
