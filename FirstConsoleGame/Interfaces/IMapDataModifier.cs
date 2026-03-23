using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstConsoleGame
{
	public interface IMapDataModifier
	{
		// Too many Authority
		public void SetEntity(Entity entity, MyVector pos);
		public void SetNewEntity<T>(MyVector pos) where T : Entity, new();
		public Entity GetEntity(int x, int y);
		public Dictionary<string, int> GetMapData();
		public void Swap(MyVector v1, MyVector v2);
		public MyVector GetSize();
		public void UpdateMapData();
		public void SetClear(bool value);
	}
}
