using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static FirstConsoleGame.Program;

namespace FirstConsoleGame
{
	public class Player : Entity
	{
		private int maxHp;
		private int hp;
		private int money;
		private Dictionary<ItemID, int> inventory;
		public Dictionary<ItemID, int> Inventory { get => inventory; }
		private int maxInvenCell = InventoryBox.elemCol * InventoryBox.elemRow;
		public int Money { get => money; }
		public int MaxHp { get => maxHp; }
		public int Hp { get => hp; }
		private bool isDead = false;
		public bool IsDead { get => isDead; }
		public override void Init(MyVector pos)
		{
			base.Init(pos);
			inventory = new Dictionary<ItemID, int>();
			symbol = 'P';
			maxHp = 8;
			hp = 3;
			isDead = false;
			money = 10;
		}
		public void InitWithRawData(RawPlayerData data)
		{
			// inven
			hp = data.hp;
			money = data.money;
			inventory = new Dictionary<ItemID, int>();
			for(int i = 0; i < data.InventoryCnt.Count; i++)
				inventory.Add(data.InventoryItemID[i], data.InventoryCnt[i]);
		}
		public void TakeDamage(int damage)
		{
			hp -= damage;
			if (hp > maxHp)
				hp = maxHp;
			if (hp <= 0)
				Die();
		}
		private void Die()
		{
			isDead = true;
		}
		public MyVector GetMove(char dir, MyVector mapSize)
		{
			MyVector tomove = new MyVector(pos.x + Utility.charToDeltaPos[dir].x, pos.y + Utility.charToDeltaPos[dir].y);
			if (mapSize.IsOutOfSquare(tomove))
			{
				tomove = pos;
			}

			return tomove;
		}
		public override string OnPlayerEnter()
		{
			return "Fail to move: out of map";
		}
		public override EntityEnum GetEntityEnum()
		{
			return EntityEnum.Player;
		}
	
		public bool CanBuy(ShopItemData itemData)
		{
			return itemData.price <= money && (inventory.ContainsKey(itemData.id) || inventory.Count < maxInvenCell);
		}
		public void AddMoney(int amount)
		{
			money += amount;
		}
		public bool BuyShopItem(ShopItemData itemData)
		{
			if (CanBuy(itemData) == false) return false;

			bool isExist = inventory.ContainsKey(itemData.id);
			if(isExist)
			{
				inventory[itemData.id]++;
			}
			else 
			{
				inventory.Add(itemData.id, 1);
			}
			money -= itemData.price;

			return true;
		}
		public void UseItem()
		{

		}
	}
}
