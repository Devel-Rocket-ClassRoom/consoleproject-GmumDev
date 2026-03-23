
namespace FirstConsoleGame
{
	public class DungeonGame_Map : IMapDataModifier
	{
		/* Too many roles...

		*/
		EntityManager entityManager;


		private MyVector gridPos; // pos of Stage grid. init at MapFactory.GetEmptyMap 
		public MyVector GridPos { get => gridPos; }

		// ----- map data
		private Entity[,] buf;
		public MyVector size;

		Dictionary<string, int> mapData = new Dictionary<string, int>();

		// ----- difficulty
		public float difficulty = 1f;
		private const float monster_rate = 0.04f;
		private const float fence_rate = 0.17f;
		public int InitMonsterNum { get => (int)((size.x - 1) * (size.y - 1) * monster_rate * difficulty); }
		public int InitFenceNum { get => (int)((size.x - 1) * (size.y - 1) * fence_rate * difficulty); }

		// -----
		public delegate bool ClearChecker();
		public ClearChecker IsClear;

		// -----
		public DungeonGame_Map(MyVector size, MyVector gridPos, float difficulty)
		{
			this.size = new MyVector(size.x, size.y);
			this.gridPos = gridPos;
			this.difficulty = difficulty;

			buf = new Entity[size.y, size.x];
			entityManager = EntityManager.GetInstance();
		}

		// ----- GET / SET, Interface Implements 
		public void SetClear(bool value)
		{
			IsClear = () => value;
		}
		public void SetEntity(Entity entity, MyVector pos)
		{
			buf[pos.y, pos.x] = entity;
		}
		public void SetNewEntity<T>(MyVector pos) where T : Entity, new()
		{
			T entity = (T)entityManager.GetNewInstance<T>(pos);
			SetEntity(entity, pos);
		}
		public Entity GetEntity(int x, int y)
		{
			return buf[y, x];
		}
		public MyVector GetSize()
		{
			return size;
		}
		public Dictionary<string, int> GetMapData()
		{
			return mapData;
		}
		public void Swap(MyVector v1, MyVector v2)
		{
			(buf[v1.y, v1.x], buf[v2.y, v2.x]) = (buf[v2.y, v2.x], buf[v1.y, v1.x]);
			buf[v1.y, v1.x].pos = v1;
			buf[v2.y, v2.x].pos = v2;
		}
		public void UpdateMapData()
		{
			mapData.Clear();
			mapData.Add("width", size.x);
			mapData.Add("height", size.y);

			int monster_cnt = 0;
			for (int r = 0; r < size.y; r++)
			{
				for (int c = 0; c < size.x; c++)
				{
					if (buf[r, c] is Monster)
					{
						mapData.Add("monster_" + monster_cnt + "_x", c);
						mapData.Add("monster_" + monster_cnt + "_y", r);
						monster_cnt++;
					}
					else if (buf[r, c] is Player)
					{
						mapData.Add("player_x", c);
						mapData.Add("player_y", r);
					}
				}
			}
			mapData.Add("monster_cnt", monster_cnt);
		}

		// -----
		public void Update()
		{
			foreach (var entity in buf)
			{
				entity.IsUpdatedOnCurFrame = false;
			}

			foreach (var entity in buf)
			{
				if (entity.IsUpdatedOnCurFrame == false)
				{
					entity.IsUpdatedOnCurFrame = true;
					entity.Update();
				}
			}
		}
		public MyVector[] GetDoorCardinalPos()
		{
			MyVector[] doorPosDatas = new MyVector[4];
			doorPosDatas[(int)DirIndex.Left] = new MyVector(0, size.y / 2);
			doorPosDatas[(int)DirIndex.Right] = new MyVector(size.x - 1, size.y / 2);
			doorPosDatas[(int)DirIndex.Up] = new MyVector(size.x / 2, 0);
			doorPosDatas[(int)DirIndex.Down] = new MyVector(size.x / 2, size.y - 1);

			return doorPosDatas;
		}
		public Door GetDoor(DirIndex dir)
		{
			MyVector pos = GetDoorCardinalPos()[(int)dir];
			return (Door)GetEntity(pos.x, pos.y);
		}
		public void SetDoorAndConnectTo(DirIndex dir, DungeonGame_Map map)
		{
			MyVector[] doorPosDatas = GetDoorCardinalPos();

			MyVector pos = doorPosDatas[(int)dir];
			SetNewEntity<Door>(pos);
			((Door)GetEntity(pos.x, pos.y)).ConnectTo(map);
		}

		public void SetEmptyMap()
		{
			for (int i = 0; i < size.y; i++)
			{
				for (int j = 0; j < size.x; j++)
				{
					SetNewEntity<EmptyEntity>(new MyVector(j, i));

					if (i == 0 || i == size.y - 1) SetNewEntity<Fence>(new MyVector(j, i));
					else if (j == 0 || j == size.x - 1) SetNewEntity<Fence>(new MyVector(j, i));
				}
			}
		}
		public void SetPlayerPosOnNewMap(Player player, DirIndex lastMoveDir)
		{
			//----------- set player position by last move, add door player last passed-through
			player.pos = new MyVector(size.x / 2, size.y / 2);

			switch (lastMoveDir)
			{
				case DirIndex.Left: player.pos.x = size.x - 2; break;
				case DirIndex.Right: player.pos.x = 1; break;
				case DirIndex.Up: player.pos.y = size.y - 2; break;
				case DirIndex.Down: player.pos.y = 1; break;
			}
			buf[player.pos.y, player.pos.x] = player;
		}

	}
}
