using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstConsoleGame
{
	public readonly struct ShopItemData
	{
		public readonly string name;
		public readonly int price;
		public readonly char symbol;
		public readonly int count;
		public ShopItemData(string name, char symbol, int price, int count)
		{
			this.name = name;
			this.symbol = symbol;
			this.price = price;
			this.count = count;
		}
	}
}
