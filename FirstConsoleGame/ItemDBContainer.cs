using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FirstConsoleGame
{
	public class ItemDBContainer
	{
		private RawItemData[] datas;
		private static ItemDBContainer instance;
		private ItemDBContainer()
		{
			try
			{
				datas = Utility.LoadData<RawItemData[]>("ItemData.json");
			}
			catch
			{
				datas = new RawItemData[3];
				datas[(int)ItemID.Potion] = new RawItemData("Potion", '8', 2, "Healing potion. Gain 2 hp");
				datas[(int)ItemID.BigPotion] = new RawItemData("Cookie", 'C', 1, "Delicious");
				datas[(int)ItemID.SmallPotion] = new RawItemData("Chicken", '9', 3, "Very Delicious.");
				
			}
			
		}
		public static ItemDBContainer GetInstance()
		{
			if (instance == null)
				instance = new ItemDBContainer();
			return instance;
		}

		public RawItemData GetItemDataById(ItemID id)
		{
			return datas[(int)id];
		}
		public ShopItemData GetShopItemData(ItemID id, int cnt)
		{
			var item = GetItemDataById(id);
			return new ShopItemData(id, item.name, item.symbol, item.price, cnt);
		}
	}
}
