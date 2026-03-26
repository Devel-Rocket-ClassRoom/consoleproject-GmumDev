using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstConsoleGame.ReadyForUnity
{
	public class StageFactory : IStageFactory
	{
		public MapFactory mapFactory;
		public IMap[] mapPrefabs;
		int stageWidth;
		int stageHeight;
		public IStage GenerateStage<T>() where T: IStage, new()
		{

			IStage stage = new T();

			for(int i = 0; i < stageHeight; i++)
			{
				for(int j = 0; j < stageWidth; j++)
				{
					int idx = Utility.rand.Next() % mapPrefabs.Length;

					IMap mapPrefab = mapPrefabs[idx];

					IMap map = mapFactory.GenerateDefaultMap<Map>();

					// fill map somehow 

					stage.StageData.mapDatas[i, j] = map.MapData;
				}
			}

			return stage;
		}
	}
}
