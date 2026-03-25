using System.Data;
using static FirstConsoleGame.Utility;

namespace FirstConsoleGame
{
	public class StageFactory
	{
		private static StageFactory instance;
		private MapFactory mapFactory;
		public const int GRID_WIDTH = 7;
		public const int GRID_HEIGHT = 7;

		private Map[,] mapGrid = new Map[GRID_HEIGHT, GRID_WIDTH];
		private MyVector gridSize = new MyVector(GRID_WIDTH, GRID_HEIGHT);

		private MyVector startPos;
		public MyVector StartPos { get => startPos; }

		private MyVector endPos;
		public MyVector EndPos { get => endPos; }

		private MyVector maxMapSize;
		public MyVector MaxMapSize { get => maxMapSize; }

		private MyVector minMapSize;
		public MyVector MinMapSize { get => minMapSize; }
		public delegate bool ClearChecker();
		public ClearChecker IsClear;
		// -----
		private StageFactory()
		{
			mapFactory = MapFactory.GetInstance();
			// never bigger than (MAX_BUF_WIDTH - 5, MAX_BUF_HEIGHT - 3)????????????????? over flow
			maxMapSize = new MyVector(60, MAX_BUFFER_HEIGHT - 5);
			maxMapSize.x = Math.Min(maxMapSize.x, MAX_BUFFER_WIDTH - 20);
			maxMapSize.y = Math.Min(maxMapSize.y, MAX_BUFFER_HEIGHT - 5);

			// never smaller than (4, 4). 
			minMapSize = new MyVector(7, 7);
			minMapSize.x = Math.Max(minMapSize.x, 4);
			minMapSize.y = Math.Max(minMapSize.y, 4);
		}
		public static StageFactory GetInstance()
		{
			if (instance == null)
				instance = new StageFactory();
			return instance;
		}

		// -----
		public Map GetMap(MyVector v) { return mapGrid[v.y, v.x]; }
		private MyVector GetRandMapSize()
		{
			return new MyVector(
					minMapSize.x + rand.Next() % (maxMapSize.x - minMapSize.x),
					minMapSize.y + rand.Next() % (maxMapSize.y - minMapSize.y));
		}
		private int GetRandRoomNum(int min, int max)
		{
			if (min < max)
				return Math.Max(Math.Min(rand.Next() % (max - min) + min, GRID_HEIGHT * GRID_WIDTH), 3);

			return 3;
		}
		private (MyVector, MyVector, HashSet<MyVector>, int) GetNewMapPath()
		{
			int roomNum = GetRandRoomNum(3, 7);
			MyVector startRoomPos = new MyVector(rand.Next() % GRID_WIDTH, rand.Next() % GRID_HEIGHT);

			// Returns min distance between two room pos. 
			Func<MyVector, MyVector, int> GetDistance = (MyVector v1, MyVector v2) => Math.Abs(v2.x - v1.x) + Math.Abs(v2.y - v1.y);

			// endRoomPosList has roomPos, which can be endRoom. 
			List<MyVector> endRoomPosList = new List<MyVector>();
			for (int r = 0; r < GRID_HEIGHT; r++)
			{
				for (int c = 0; c < GRID_WIDTH; c++)
				{
					int dist = GetDistance(startRoomPos, new MyVector(c, r));
					if (dist > 0 && dist < roomNum)
					{
						endRoomPosList.Add(new MyVector(c, r));
					}
				}
			}
			MyVector endRoomPos = endRoomPosList[rand.Next() % endRoomPosList.Count];
			roomNum -= GetDistance(endRoomPos, startRoomPos) + 1;


			// Set start~end map exclusive(main path maps)
			HashSet<MyVector> mainPaths = new HashSet<MyVector>();
			MyVector pos = startRoomPos;
			while (GetDistance(endRoomPos, pos) > 0)
			{
				mainPaths.Add(pos);
				MyVector cardinal_8_dir = new MyVector(endRoomPos.x - pos.x, endRoomPos.y - pos.y);
				if (cardinal_8_dir.x == 0)
					pos.y += cardinal_8_dir.y > 0 ? 1 : -1;
				else if (cardinal_8_dir.y == 0)
					pos.x += cardinal_8_dir.x > 0 ? 1 : -1;
				else
				{
					if (rand.Next() % 2 == 0) pos.x += cardinal_8_dir.x > 0 ? 1 : -1;
					else pos.y += cardinal_8_dir.y > 0 ? 1 : -1;
				}
			}
			mainPaths.Remove(startRoomPos);  // deque start pos

			// set side rooms. Not implemented
			MyVector boundary = new MyVector(GRID_WIDTH, GRID_HEIGHT);
			MyVector[] deltaPos = { new MyVector(1, 0), new MyVector(-1, 0), new MyVector(0, 1), new MyVector(0, -1) };

			return (startRoomPos, endRoomPos, mainPaths, roomNum);
		}
		public void SetNewStage(Player player)
		{
			(startPos, endPos, HashSet<MyVector> mainPaths, int roomNum) = GetNewMapPath();

			mapGrid = new Map[GRID_HEIGHT, GRID_WIDTH];

			foreach (var pos in mainPaths)
			{
				mapGrid[pos.y, pos.x] = mapFactory.GetEmptyMap(GetRandMapSize(), pos);
			}
			mapGrid[startPos.y, startPos.x] = mapFactory.GetEmptyMap(minMapSize, startPos);
			mapGrid[endPos.y, endPos.x] = mapFactory.GetEmptyMap(maxMapSize, endPos);

			mainPaths.Add(startPos);
			mainPaths.Add(endPos);

			while (roomNum > 0)
			{
				int idx = rand.Next() % mainPaths.Count;
				MyVector crosRoadPos = mainPaths.ElementAt(idx);
				MyVector sidemapPos;

				sidemapPos = crosRoadPos + Utility.dirToDeltaPos[Utility.intToDirIndex[rand.Next() % 4]];

				if (gridSize.IsOutOfSquare(sidemapPos) == false && mainPaths.Contains(sidemapPos) == false)
				{
					mainPaths.Add(sidemapPos);
					mapGrid[sidemapPos.y, sidemapPos.x] = mapFactory.GetEmptyMap(GetRandMapSize(), sidemapPos);
					roomNum--;
				}
			}

			SetDoorsWithPath(mainPaths);
			RandomlyFillUpMaps(mainPaths);

			IsClear = () =>
			{
				bool flag = true;
				for (int i = 0; i < gridSize.y; i++)
				{
					for (int j = 0; j < gridSize.x; j++)
					{
						if (mapGrid[i, j] == null) continue;

						flag = flag && mapGrid[i, j].IsClear();
					}
				}
				return flag;
			};
		}
		public void SetNewStageWithRawData(RawStageData data)
		{
			mapGrid = new Map[GRID_HEIGHT, GRID_WIDTH];

			HashSet<MyVector> mainPaths = new HashSet<MyVector>();

			foreach (var mapdata in data.mapdatas)
			{
				if (mapdata.isEndMap) endPos = new MyVector(mapdata.gridx, mapdata.gridy);
				else if (mapdata.isStartMap) startPos = new MyVector(mapdata.gridx, mapdata.gridy);
				
				mapGrid[mapdata.gridy, mapdata.gridx] = RawDataParser.GetMapFromRawData(mapdata);
				mainPaths.Add(new MyVector(mapdata.gridx, mapdata.gridy));
			}

			SetDoorsWithPath(mainPaths);
			// dont fill up maps. since, map already filled with RawData.
			// dont resolve below comment
			// RandomlyFillUpMaps(mainPaths);

			IsClear = () =>
			{
				bool flag = true;
				for (int i = 0; i < gridSize.y; i++)
				{
					for (int j = 0; j < gridSize.x; j++)
					{
						if (mapGrid[i, j] == null) continue;

						flag = flag && mapGrid[i, j].IsClear();
					}
				}
				return flag;
			};
		}
		private void RandomlyFillUpMaps(HashSet<MyVector> mainPaths)
		{
			foreach (var pos in mainPaths)
			{
				Map curmap = mapGrid[pos.y, pos.x];

				if (pos.x == startPos.x && pos.y == startPos.y)
					mapFactory.FillUpMap_Start(curmap);
				else if (pos.x == endPos.x && pos.y == endPos.y)
					mapFactory.FillUpMap_End(curmap);
				else
				{
					int randomValue = rand.Next() % mapFactory.FillUpMapTasks.Count;
					mapFactory.FillUpMapTasks[randomValue](curmap);
				}
			}
		}
		private void SetDoorsWithPath(HashSet<MyVector> mainPaths)
		{
			foreach (var pos in mainPaths)
			{
				Map curmap = mapGrid[pos.y, pos.x];
				foreach (DirIndex dir in Enum.GetValues<DirIndex>())
				{
					MyVector nearbyPos = pos + Utility.dirToDeltaPos[dir];
					if (gridSize.IsOutOfSquare(nearbyPos)) continue;
					Map targetmap = mapGrid[nearbyPos.y, nearbyPos.x];
					if (targetmap != null)
					{
						curmap.SetDoorAndConnectTo(dir, targetmap);
					}
				}
			}
		}
	}
}
