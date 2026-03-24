
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FirstConsoleGame.Utility;

namespace FirstConsoleGame
{
	/*
	
	
	Itemshop with AlertBoxes. 
	Itemshop has 2 row * 3 col grid...
	{{A G D}
	{1 2 3}}
	A and D is to select item 1, 2, 3
	G to buy
	Each selection(A, G, D) has and Invokes other alertBox. 

	 
	 
	 */
	public class ItemShop : Entity
	{
		private ShopAlertRenderBox[] shopBox;


		private const int maxItemCnt = 3;
		private ShopItemData[] items;
		private int[] leftItemStock;
		private int curItemCnt;

		public override void Init(MyVector pos)
		{
			base.Init(pos);
			symbol = 'S';
			items = new ShopItemData[maxItemCnt];
			leftItemStock = new int[maxItemCnt];
			curItemCnt = 0;

			var margin = AlertRenderer.GetInstance().Margin;
			var size = AlertRenderer.GetInstance().Size;

			shopBox = new ShopAlertRenderBox[3];

			shopBox[0] = new ShopAlertRenderBox(margin, size, "Shop");
			shopBox[1] = new ShopAlertRenderBox(margin, size, "Shop");
			shopBox[2] = new ShopAlertRenderBox(margin, size, "Shop");

			string left = " Exit <-------";
			string buy = " G to Buy";
			string right = " -------> Exit";

			shopBox[0].SetCallback('A', left, () => { });
			shopBox[0].SetCallback('G', buy, BuyLeftItem);
			shopBox[0].SetCallback('D', right, ActivateMiddleShop);

			shopBox[1].SetCallback('A', left, ActivateLeftShop);
			shopBox[1].SetCallback('G', buy, BuyMiddleItem);
			shopBox[1].SetCallback('D', right, ActivateRightShop);

			shopBox[2].SetCallback('A', left, ActivateMiddleShop);
			shopBox[2].SetCallback('G', buy, BuyRightItem);
			shopBox[2].SetCallback('D', right, () => { });

		}

		public void LocalRender()
		{
			for(int i = 0; i < 3; i++)
			{
				shopBox[i].LocalRender();
			}
		}
		private void ActivateLeftShop()
		{
			AlertRenderer.GetInstance().AlertShopBox = shopBox[0];
			shopBox[0].Alert();
		}
		private void ActivateMiddleShop()
		{
			AlertRenderer.GetInstance().AlertShopBox = shopBox[1];
			shopBox[1].Alert();
		}
		private void ActivateRightShop()
		{
			AlertRenderer.GetInstance().AlertShopBox = shopBox[2];
			shopBox[2].Alert();
		}

		private void BuyItem(int idx)
		{
			ShopItemData item = items[idx];
			if (leftItemStock[idx] > 0)
			{
				// check player money

				leftItemStock[idx]--;
				if (leftItemStock[idx] <= 0)
				{
					for (int i = 0; i < 3; i++)
					{
						string msg = "Soldout";
						if (i == idx)
							msg = ">Soldout<";
						shopBox[i].SetCallback(item.symbol, msg, () => { });
						shopBox[i].Redraw();
					}
				}
				// reduce player money

				// add item to player inventory
			}
		}
		private void BuyLeftItem()
		{
			BuyItem(0);
		}
		private void BuyMiddleItem()
		{
			BuyItem(1);
		}
		private void BuyRightItem()
		{
			BuyItem(2);
		}
		public void SetItem(ShopItemData itemData)
		{
			if (curItemCnt > maxItemCnt - 1) return;

			AlertRenderBox.InputCallback[] callback = [ActivateLeftShop, ActivateMiddleShop, ActivateRightShop];
			for (int i = 0; i < 3; i++)
			{
				string name = itemData.name;
				if (i == curItemCnt)
					name = $">{name}<";
				int price = itemData.price;

				shopBox[i].SetCallback(itemData.symbol, $"{name} ${price}", callback[curItemCnt]);
				
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

			LocalRender();

			ActivateMiddleShop();

			return $"Thanks!";
		}
	}
}
