using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FirstConsoleGame
{
	public struct RawPlayerData
	{
		[JsonInclude]
		public int hp;
		[JsonInclude]
		public int money;
		[JsonInclude]
		public List<int> InventoryCnt;
		[JsonInclude]
		public List<ItemID> InventoryItemID;
	}
}
