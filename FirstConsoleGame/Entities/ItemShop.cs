using FirstConsoleGame.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FirstConsoleGame.Utility;

namespace FirstConsoleGame
{
	public class ItemShop : Entity
	{
		private AlertRenderBox[] shopBox;


		private const int maxItemCnt = 3;
		private ItemData[] items;
		private int[] leftItemStock;
		private int curItemCnt;
		public override void Init(MyVector pos)
		{
			base.Init(pos);
			symbol = 'S';
			items = new ItemData[maxItemCnt];
			leftItemStock = new int[maxItemCnt];
			curItemCnt = 0;

			var margin = new MyVector(MAX_BUFFER_WIDTH / 8, MAX_BUFFER_HEIGHT / 6);
			var size = new MyVector(MAX_BUFFER_WIDTH - margin.x * 2, MAX_BUFFER_HEIGHT - margin.y * 2);

			shopBox = new AlertRenderBox[3];

			shopBox[0] = new AlertRenderBox(margin, size, "Shop");
			shopBox[1] = new AlertRenderBox(margin, size, "Shop");
			shopBox[2] = new AlertRenderBox(margin, size, "Shop");

			shopBox[0].SetCallback('A', "<", () => { });
			shopBox[0].SetCallback('G', "---Buy---", BuyLeftItem);
			shopBox[0].SetCallback('D', ">", ActivateMiddleShop);

			shopBox[1].SetCallback('A', "<", ActivateLeftShop);
			shopBox[1].SetCallback('G', "---Buy---", BuyMiddleItem);
			shopBox[1].SetCallback('D', ">", ActivateRightShop);

			shopBox[2].SetCallback('A', "<", ActivateMiddleShop);
			shopBox[2].SetCallback('G', "---Buy---", BuyRightItem);
			shopBox[2].SetCallback('D', ">", () => { });
		}

		private void ActivateLeftShop()
		{
			AlertRenderer.GetInstance().alertShopBox = shopBox[0];
			shopBox[0].Alert();
		}
		private void ActivateMiddleShop()
		{
			AlertRenderer.GetInstance().alertShopBox = shopBox[1];
			shopBox[1].Alert();
		}
		private void ActivateRightShop()
		{
			AlertRenderer.GetInstance().alertShopBox = shopBox[2];
			shopBox[2].Alert();
		}

		private void BuyLeftItem()
		{
			if (leftItemStock[0] > 0)
			{
				// check player money

				ItemData item = items[0];
				leftItemStock[0]--;
				// reduce player money


				// add item to player inventory
			}
			
			shopBox[0].Alert();
		}
		private void BuyMiddleItem()
		{

			shopBox[1].Alert();
		}
		private void BuyRightItem()
		{

			shopBox[2].Alert();
		}
		public void SetItems(ItemData itemData)
		{
			if (curItemCnt > maxItemCnt - 1) return;


			for(int i = 0; i < 3; i++)
			{
				string name = itemData.name;
				if (i == curItemCnt) 
					name = $">{name}<";
				int price = itemData.price;
				// if player press symbol char, then activate middle shop. is just not to quit shop. 
				shopBox[i].SetCallback(itemData.symbol, $"{name} {price}\\", ActivateMiddleShop);
			}

			items[curItemCnt] = itemData;
			leftItemStock[curItemCnt] = itemData.count;
			curItemCnt++;
		}
		public override string OnPlayerEnter()
		{
			var mapdata = map.GetMapData();
			MyVector playerPos = new MyVector(mapdata["player_x"], mapdata["player_y"]);
			Player player = (Player)map.GetEntity(playerPos.x, playerPos.y);

			AlertRenderer.GetInstance().alertShopBox = shopBox[1];
			shopBox[1].Alert();

			return "You bought something!";
		}
	}
}
