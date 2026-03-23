
using static FirstConsoleGame.Utility;
namespace FirstConsoleGame
{
	public class DungeonGame_MapFactory
	{

		// ----- Singleton + Constructor
		private static DungeonGame_MapFactory instance;
		EntityManager entityManager;
		private DungeonGame_MapFactory()
		{
			entityManager = EntityManager.GetInstance();
			GetMapTasks = new List<GetMapDelegate>();
			GetMapTasks.Add(FillUpMap_Default);
			GetMapTasks.Add(FillUpMap_Snake);
			GetMapTasks.Add(FillUpMap_SnakeGrowUp);
			GetMapTasks.Add(FillUpMap_BigSnake);
		}
		public static DungeonGame_MapFactory GetInstance()
		{
			if (instance == null)
			{
				instance = new DungeonGame_MapFactory();
			}

			return instance;
		}

		public delegate void GetMapDelegate(DungeonGame_Map map);
		public List<GetMapDelegate> GetMapTasks;

		// -----
		public DungeonGame_Map GetEmptyMap(MyVector mapSize, MyVector gridPos)
		{
			DungeonGame_Map map = new DungeonGame_Map(mapSize, gridPos, difficulty: 1f);

			map.SetEmptyMap();

			return map;
		}

		public void FillUpMap_Default(DungeonGame_Map map)
		{
			HashSet<(int, int)> monsterPos = new HashSet<(int, int)>();
			List<MyVector> endPos = new List<MyVector>();
			GetRandomEmptyPos(map, ref monsterPos, map.InitMonsterNum);
			foreach (var pos in monsterPos)
			{
				map.SetNewEntity<Monster>(new MyVector(pos.Item1, pos.Item2));
				endPos.Add(new MyVector(pos.Item1, pos.Item2));
			}
			map.IsClear = () => map.GetMapData()["monster_cnt"] == 0;

			foreach (var pos in map.GetDoorCardinalPos())
			{
				if (map.GetEntity(pos.x, pos.y) is Door)
					endPos.Add(pos);
			}

			SetRandomFences(map, map.GetDoorCardinalPos(), endPos.ToArray(), map.InitFenceNum);

			map.UpdateMapData();
		}
		public void FillUpMap_SnakeGrowUp(DungeonGame_Map map)
		{
			HashSet<(int, int)> monsterPos = new HashSet<(int, int)>();
			List<MyVector> endPos = new List<MyVector>();
			GetRandomEmptyPos(map, ref monsterPos, map.InitMonsterNum);
			foreach (var pos in monsterPos)
			{
				if (rand.Next() % 3 == 0)
					map.SetNewEntity<SnakeBaby>(new MyVector(pos.Item1, pos.Item2));
				else
					map.SetNewEntity<Monster>(new MyVector(pos.Item1, pos.Item2));
				endPos.Add(new MyVector(pos.Item1, pos.Item2));
			}
			map.IsClear = () => map.GetMapData()["monster_cnt"] == 0;

			foreach (var pos in map.GetDoorCardinalPos())
			{
				if (map.GetEntity(pos.x, pos.y) is Door)
					endPos.Add(pos);
			}

			SetRandomFences(map, map.GetDoorCardinalPos(), endPos.ToArray(), map.InitFenceNum);

			map.UpdateMapData();
		}
		public void FillUpMap_Snake(DungeonGame_Map map)
		{
			HashSet<(int, int)> monsterPos = new HashSet<(int, int)>();
			GetRandomEmptyPos(map, ref monsterPos, map.InitMonsterNum);
			foreach (var pos in monsterPos)
			{
				map.SetNewEntity<Snake_Head>(new MyVector(pos.Item1, pos.Item2));
			}
			map.IsClear = () => false;  // Snake monster gives u clear token. 

			map.UpdateMapData();
		}
		public void FillUpMap_BigSnake(DungeonGame_Map map)
		{
			HashSet<(int, int)> monsterPos = new HashSet<(int, int)>();
			GetRandomEmptyPos(map, ref monsterPos, 1);
			foreach (var pos in monsterPos)
			{
				map.SetNewEntity<BigSnake_Head>(new MyVector(pos.Item1, pos.Item2));
			}
			map.IsClear = () => false;  // Snake monster gives u clear token. 

			map.UpdateMapData();
		}
		public void FillUpMap_Start(DungeonGame_Map map)
		{
			map.IsClear = () => true;
			HashSet<(int, int)> healingPotionPos = new HashSet<(int, int)>();
			GetRandomEmptyPos(map, ref healingPotionPos, 1);
			foreach (var pos in healingPotionPos)
			{
				map.SetNewEntity<HealingPotion>(new MyVector(pos.Item1, pos.Item2));
			}

			map.UpdateMapData();
		}
		public void FillUpMap_End(DungeonGame_Map map)
		{
			map.IsClear = () => true;
			HashSet<(int, int)> healingPotionPos = new HashSet<(int, int)>();
			GetRandomEmptyPos(map, ref healingPotionPos, 2);
			foreach (var pos in healingPotionPos)
			{
				map.SetNewEntity<HealingPotion>(new MyVector(pos.Item1, pos.Item2));
			}

			HashSet<(int, int)> NextStageBiconPos = new HashSet<(int, int)>();
			GetRandomEmptyPos(map, ref NextStageBiconPos, 1);
			foreach (var pos in NextStageBiconPos)
			{
				map.SetNewEntity<NextStageBicon>(new MyVector(pos.Item1, pos.Item2));
			}

			map.UpdateMapData();
		}

		/// <summary>
		/// Set Fences amount of many, on map that can have startPos[] and endPos[]. 
		/// startPos[] can be... 
		///		...Doors are set on the middle of each walls...4 doors for maximum. 
		/// endPos[] can be...
		///		...Monsters that must be killed to escape room. 
		///			...But SnakeMonsters dont need to be killed to escape room. So they are not contained in endPos[]
		///		...Doors since player have to reach here. 
		///	
		/// [How This Method work]
		///	Get Random Empty Space Positions. Fill that positions with Fence. 
		///	Then, Get All the reachable positions. this positions can be reached even if player dont pass through fences. 
		/// Check if reachable positions contain all the endPos. 
		///		if true, Place Fence Entity.
		///		else, do above tasks again.  
		/// 
		/// </summary>
		/// <param name="map"></param>
		/// <param name="startPos"></param>
		/// <param name="endPos"></param>
		/// <param name="many"></param>
		private void SetRandomFences(DungeonGame_Map map, MyVector[] startPos, MyVector[] endPos, int many)
		{
			HashSet<(int, int)> notAllowedPos = new HashSet<(int, int)>();  // fence positions
			HashSet<(int, int)> reachables = new HashSet<(int, int)>();     // endPositions must be subset of this.
			bool flag = false;
			do
			{
				flag = true;
				notAllowedPos.Clear();
				reachables.Clear();
				GetRandomEmptyPos(map, ref notAllowedPos, many);
				GetReachables(map, ref reachables, startPos, [FENCE_SYMBOL], notAllowedPos);
				foreach (var pos in endPos)
				{
					flag = flag && reachables.Contains(pos.GetTuple());
					if (flag == false)
						break;
				}
			} while (flag == false);

			foreach (var pos in notAllowedPos)
			{
				Fence entity = (Fence)entityManager.GetNewInstance<Fence>(new MyVector(pos.Item1, pos.Item2));
				map.SetEntity(entity, entity.pos);
			}
		}
		private HashSet<(int, int)> GetReachables(DungeonGame_Map map, ref HashSet<(int, int)> visited, MyVector[] starts, HashSet<char> notAllowedSymbols, HashSet<(int, int)> notAllowedPos)
		{

			(int, int)[] dp = { (0, 1), (0, -1), (1, 0), (-1, 0) };

			HashSet<(int, int)>[] subVisited = new HashSet<(int, int)>[starts.Length];
			for (int i = 0; i < starts.Length; i++)
			{
				Queue<(int, int)> que = new Queue<(int, int)>();
				subVisited[i] = new HashSet<(int, int)>();
				subVisited[i].Add((starts[i].x, starts[i].y));
				que.Enqueue((starts[i].x, starts[i].y));
				do
				{
					(int x, int y) = que.Dequeue();
					for (int j = 0; j < 4; j++)
					{
						(int dx, int dy) = (x + dp[j].Item1, y + dp[j].Item2);
						if (dx < 0 || dy < 0 || dx >= map.size.x || dy >= map.size.y) continue;
						if (subVisited[i].Contains((dx, dy)) || notAllowedPos.Contains((dx, dy)) || notAllowedSymbols.Contains(map.GetEntity(dx, dy).symbol))
							continue;

						que.Enqueue((dx, dy));
						subVisited[i].Add((dx, dy));
					}
				} while (que.Count > 0);
			}
			visited.UnionWith(subVisited[0]);
			for (int i = 1; i < starts.Length; i++)
			{
				visited.IntersectWith(subVisited[i]);
			}

			return visited;
		}
		private void GetRandomEmptyPos(DungeonGame_Map map, ref HashSet<(int, int)> visited, int cnt)
		{
			while (cnt > 0)
			{
				int x = rand.Next() % map.size.x;
				int y = rand.Next() % map.size.y;

				// cannot overlap. char c only set to ' ': empty space. 
				if (map.GetEntity(x, y) is EmptyEntity)
				{
					cnt--;
					visited.Add((x, y));
				}
			}
		}
	}
}
