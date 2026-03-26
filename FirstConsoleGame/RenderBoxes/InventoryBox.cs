using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace FirstConsoleGame
{
	public class InventoryBox: RenderBox
	{
		public const int elemCol = 3;
		public const int elemRow = 3;
		int elemHeight = 3;

		MyVector gridMargin;
		MyVector gridCellSize;
		MyVector cellPadding;
		MyVector descriptMargin;
		MyVector descriptPadding;
		MyVector descriptSize;

		int lastInvenCnt = 0;

		char[,] cell;


		public InventoryBox(MyVector size) : base(size) { }

		public InventoryBox(MyVector margin, MyVector size) : base(margin, size) { }
		public override void Init()
		{
			gridMargin = new MyVector(1, 1);
			cellPadding = new MyVector(1, 1);
			gridCellSize = new MyVector((size.x - gridMargin.x*2)/elemCol, elemHeight + cellPadding.y*2 + 2);
			cell = new char[gridCellSize.y, gridCellSize.x];
			for (int i = 0; i < gridCellSize.y; i++)
			{
				for (int j = 0; j < gridCellSize.x; j++)
				{
					if (i == 0 || i == gridCellSize.y - 1)
						cell[i, j] = '─';
					else if (j == 0 || j == gridCellSize.x - 1)
						cell[i, j] = '│';
					else
						cell[i, j] = ' ';
				}
			}
			cell[0, 0] = '┌';
			cell[gridCellSize.y - 1, 0] = '└';
			cell[0, gridCellSize.x - 1] = '┐';
			cell[gridCellSize.y - 1, gridCellSize.x - 1] = '┘';
			// outer

			descriptMargin = new MyVector(1, gridCellSize.y*elemRow + 1);
			descriptPadding = new MyVector(1, 1);
			descriptSize = new MyVector(size.x - descriptMargin.x * 2, size.y - descriptMargin.y - 1);
			// inner 

			DrawBorder();
			DrawGrid();
		}
		public override void DrawBorder()
		{
			DrawBorder(" [Inventory]");
		}
		private void DrawCell(int x, int y)
		{
			for (int i = 0; i < gridCellSize.y; i++)
			{
				for(int j = 0; j < gridCellSize.x; j++)
				{
					buf[y + i, x + j] = cell[i, j];
				}
			}
		}
		private void DrawGrid()
		{
			ClearGrid();
			for (int i = 0; i < elemRow; i++)
			{
				for (int j = 0; j < elemCol; j++)
				{
					MyVector pivot = new MyVector(gridMargin.x + j * gridCellSize.x, gridMargin.y + i * gridCellSize.y);

					DrawCell(pivot.x, pivot.y);

				}
			}
		}
		public void Update(Dictionary<ItemID, int> inventory)
		{
			if (lastInvenCnt == inventory.Count)
			{
				return;
			}
			DrawGrid();

			lastInvenCnt = inventory.Count;

			var elems = inventory.ToArray();

			for (int i = 0; i < elemRow; i++)
			{
				for (int j = 0; j < elemCol; j++)
				{
					int idx = j + i * elemCol;
					if (idx >= elems.Length) return;

					MyVector pivot = new MyVector(gridMargin.x + j * gridCellSize.x, gridMargin.y + i * gridCellSize.y);
					

					ItemID id = elems[idx].Key;
					int cnt = elems[idx].Value;

					var rawData = ItemDBContainer.GetInstance().GetItemDataById(id);

					pivot += cellPadding;
					buf[pivot.y, pivot.x] = '[';
					buf[pivot.y, pivot.x + 1] = rawData.symbol;
					buf[pivot.y, pivot.x + 2] = ']';
					for (int k = 0; k < rawData.name.Length && k < gridCellSize.x - cellPadding.x * 2; k++)
						buf[pivot.y, pivot.x + 4 + k] = rawData.name[k];

					string priceStr = $" ${rawData.price}";
					for (int k = 0; k < priceStr.Length && k < gridCellSize.x - cellPadding.x * 2; k++)
						buf[pivot.y + 1, pivot.x + k] = priceStr[k];
				}
			}
		}
		public override void Render(char[,] parentbuf)
		{
			for (int r = 0; r < size.y; r++)
			{
				for (int c = 0; c < size.x; c++)
				{
					int globalx = c;
					int globaly = r;
					parentbuf[r + margin.y, c + margin.x] = buf[r, c];
				}
			}
		}
		private void ClearGrid()
		{
			for (int r = 1; r < size.y - 1; r++)
			{
				for (int c = 1; c < size.x - 1; c++)
				{
					buf[r, c] = ' ';
				}
			}
		}
	}
}
