
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
		private AlertRenderBox[] shopBox;


		private const int maxItemCnt = 3;
		private ItemData[] items;
		private int[] leftItemStock;
		private int curItemCnt;
		private string outputStr = "";

		public override void Init(MyVector pos)
		{
			base.Init(pos);
			symbol = 'S';
			items = new ItemData[maxItemCnt];
			leftItemStock = new int[maxItemCnt];
			curItemCnt = 0;
			outputStr = "";

			var margin = AlertRenderer.GetInstance().Margin;
			var size = AlertRenderer.GetInstance().Size;

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
			ItemData item = items[idx];
			if (leftItemStock[idx] > 0)
			{
				// check player money

				leftItemStock[idx]--;
				if (leftItemStock[idx] <= 0)
				{
					for (int i = 0; i < 3; i++)
					{
						string msg = "  Soldout  ";
						if (i == idx)
							msg = " >Soldout< ";
						shopBox[i].SetCallback(item.symbol, msg, () => { });
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
		public void SetItem(ItemData itemData)
		{
			if (curItemCnt > maxItemCnt - 1) return;


			AlertRenderBox.InputCallback[] callback = [ActivateLeftShop, ActivateMiddleShop, ActivateRightShop];
			for (int i = 0; i < 3; i++)
			{
				string name = itemData.name;
				if(name == "Soldout")
				{
					if (i == curItemCnt)
						name = $">{name}<";
					shopBox[i].SetCallback(itemData.symbol, name, callback[curItemCnt]);
				}
				else
				{
					if (i == curItemCnt)
						name = $">{name}<";
					int price = itemData.price;

					shopBox[i].SetCallback(itemData.symbol, $"{name} ${price}", callback[curItemCnt]);
				}
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
