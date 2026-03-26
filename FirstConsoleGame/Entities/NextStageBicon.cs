using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstConsoleGame
{
	public class NextStageBicon : Entity
	{
		// bicon to go next stage if stage.clear 
		public override void Init(MyVector pos)
		{
			base.Init(pos);
			symbol = '*';
		}
		public override string OnPlayerEnter()
		{
			return "You can't go next stage now!!";
		}
		public override EntityEnum GetEntityEnum()
		{
			return EntityEnum.NextStageBicon;
		}
	}
}
