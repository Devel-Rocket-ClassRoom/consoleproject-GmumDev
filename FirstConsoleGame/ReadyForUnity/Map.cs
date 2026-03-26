using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstConsoleGame.ReadyForUnity
{
	public class Map : IMap
	{
		protected MapData mapdata;

		IMapFactory.FillTask fillTask;

		public MapData MapData { get => mapdata; }
		public IEntity GetEntity(int x, int y)
		{
			return mapdata.Entities[y, x];
		}
		public bool SetEntity(IEntity entity, int x, int y)
		{
			if (entity == null) return false;
			mapdata.Entities[y, x] = entity;
			return true;
		}
		public void SetNewEntity<T>(int x, int y) where T : IEntity, new()
		{
			IEntity entity = new T();
			mapdata.Entities[y, x] = entity;
		}
	}
}
