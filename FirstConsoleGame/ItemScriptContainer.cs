using System;
using System.Collections.Generic;
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
		private const string path = "../../../";
		private ItemDBContainer()
		{
			datas = JsonSerializer.Deserialize<RawItemData[]>(File.ReadAllText(path + "ItemData.json"));
;			
			
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
			return new ShopItemData(item.name, item.symbol, item.price, cnt);
		}
	}
}
