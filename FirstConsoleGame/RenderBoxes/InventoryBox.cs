using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstConsoleGame
{
	public class InventoryBox: RenderBox
	{
		const int elemCol = 3;
		const int elemRow = 3;
		const int elemCount = elemCol * elemRow;
		string[,] itemNameInventory = new string[elemRow, elemCol];
		int[,] itemCntInventory = new int[elemRow, elemCol];

		// [elem name]
		// symbol cnt
		// description

		public InventoryBox(MyVector size) : base(size) { }

		public InventoryBox(MyVector margin, MyVector size) : base(margin, size) { }
		public override void Init()
		{
			DrawBorder(" [Inventory]");
		}
		public void Update()
		{

		}
		public void AddItem(string name, int cnt)
		{

		}
		public override void Render(char[,] parentbuf)
		{
			int divx = size.x / elemCol;
			int divy = size.y / elemRow;
 

		}

	}
}
