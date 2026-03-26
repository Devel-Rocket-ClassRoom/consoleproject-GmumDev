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
				var obj = JsonSerializer.Deserialize<RawItemData[]>("[\r\n\t{\r\n\t\t\"name\": \"Potion\",\r\n\t\t\"symbol\": \"8\",\r\n\t\t\"price\": 2,\r\n\t\t\"descript\": \"Healing potion. Gain 2 hp.\"\r\n\t},\r\n\t{\r\n\t\t\"name\": \"Cookie\",\r\n\t\t\"symbol\": \"C\",\r\n\t\t\"price\": 1,\r\n\t\t\"descript\": \"Delicious.\"\r\n\t},\r\n\t{\r\n\t\t\"name\": \"Chicken\",\r\n\t\t\"symbol\": \"9\",\r\n\t\t\"price\": 3,\r\n\t\t\"descript\": \"Very Delicious.\"\r\n\t}\r\n]");
				datas = obj;
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
