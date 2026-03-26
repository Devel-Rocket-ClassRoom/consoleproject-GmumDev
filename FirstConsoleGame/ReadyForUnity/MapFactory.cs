using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstConsoleGame.ReadyForUnity
{
	public class MapFactory : IMapFactory
	{
		public IMap mapPrefab;
		int mapWidth;
		int mapHeight;

		IMapFactory.FillTask task = FillMap;
		public IMap FillMap(IMap map)
		{

		}
		public IMap GenerateDefaultMap<T>() where T: IMap, new()
		{
			IMap map = new T();
			return map;
		}
	}
}
