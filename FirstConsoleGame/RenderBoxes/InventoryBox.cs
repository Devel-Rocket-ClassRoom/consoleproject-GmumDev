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

		MyVector gridPadding;
		MyVector gridSize;
		MyVector descriptPadding;
		MyVector descriptSize;

		// [elem name]
		// symbol cnt
		// description

		public InventoryBox(MyVector size) : base(size) { }

		public InventoryBox(MyVector margin, MyVector size) : base(margin, size) { }
		public override void Init()
		{
			gridPadding = new MyVector(1, 1);
			descriptPadding = new MyVector(1, 1);
			descriptSize = new MyVector(size.x - descriptPadding.x * 2, 5 - descriptPadding.y*2);
			gridSize = new MyVector(size.x - gridPadding.x * 2, size.y - gridPadding.y * 2 - descriptSize.y - descriptPadding.y * 2);

			DrawBorder();
		}
		public override void DrawBorder()
		{
			DrawBorder(" [Inventory]");
		}
		private void DrawGrid()
		{
			for(int r = gridPadding.y; r < gridSize.y; r++)
			{
				for(int c = gridPadding.x; c < gridSize.x; c++)
				{

				}
			}
		}
		public void Update()
		{

		}
		public void AddItem(ItemID id)
		{
			var item = ItemDBContainer.GetInstance().GetItemDataById(id);
			
		}

	}
}
