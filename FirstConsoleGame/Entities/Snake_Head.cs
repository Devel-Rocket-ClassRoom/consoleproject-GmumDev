
using static FirstConsoleGame.Utility;
namespace FirstConsoleGame
{
	public class Snake_Head : Entity
	{
		protected List<Entity> bodies;
		int lifetime;
		int stuckedTurn;
		bool isBiconCreated;
		bool isTailCutted;
		float mapClearRate = 10f;
		public override void Init(MyVector pos)
		{
			base.Init(pos);
			lifetime = 0;
			symbol = 'H';
			stuckedTurn = 0;
			isBiconCreated = false;
			isTailCutted = false;
			bodies = new List<Entity>() { this };
			AddTail();
			AddTail();
			AddTail();
		}
		protected void AddTail()
		{
			bodies.Add(EntityManager.GetInstance().GetNewInstance<Snake_Tail>(pos));
		}
		public override void Update()
		{
			base.Update();
			lifetime++;
			if (isBiconCreated == false)
			{
				if (lifetime > bodies.Count * 2)
				{
					if (rand.Next() % 100 < mapClearRate)
					{
						isBiconCreated = true;
						var lastTailPos = bodies[bodies.Count - 1].pos;
						map.SetNewEntity<MapClearBicon>(lastTailPos);
						bodies[bodies.Count - 1] = map.GetEntity(lastTailPos.x, lastTailPos.y);
					}
					else
						mapClearRate += 0.1f;
				}
			}
			else if (isTailCutted == false)
			{
				var lastTailPos = bodies[bodies.Count - 1].pos;
				Entity lastTail = map.GetEntity(lastTailPos.x, lastTailPos.y);
				if (lastTail is not MapClearBicon)  // right after player got bicon. 
				{
					bodies.RemoveAt(bodies.Count - 1);
					isTailCutted = true;
				}
			}

			// get nearby pos
			List<MyVector> emptyPos = new List<MyVector>();
			(int, int)[] dirs = new (int, int)[4] { (pos.x + 1, pos.y), (pos.x - 1, pos.y), (pos.x, pos.y + 1), (pos.x, pos.y - 1) };

			for (int i = 0; i < 4; i++)
			{
				int x = dirs[i].Item1;
				int y = dirs[i].Item2;
				if (map.GetSize().IsOutOfSquare(new MyVector(x, y)) == false)
				{
					if (map.GetEntity(x, y) is EmptyEntity)
						emptyPos.Add(new MyVector(x, y));


				}
			}

			// get random nearby empty pos
			if (emptyPos.Count == 0)
			{
				if (++stuckedTurn >= 5)
				{
					map.Swap(bodies[bodies.Count - 1].pos, bodies[0].pos);
					for (int i = 0; i < bodies.Count - 1; i++)
						map.Swap(bodies[i].pos, bodies[i + 1].pos);
				}
				return;
			}
			else
				stuckedTurn = 0;

			var moveto = emptyPos[rand.Next() % emptyPos.Count];

			// move head, and all tails
			MyVector[] originPos = new MyVector[bodies.Count];
			for (int i = 0; i < bodies.Count; i++)
			{
				originPos[i] = bodies[i].pos;
			}

			// head move
			map.Swap(pos, moveto);
			// tail move
			for (int i = 1; i < bodies.Count; i++)
			{
				if (map.GetEntity(bodies[i].pos.x, bodies[i].pos.y) is EmptyEntity)
				{
					map.SetEntity(bodies[i], bodies[i].pos);
				}
				map.Swap(bodies[i].pos, originPos[i - 1]);
				bodies[i].pos = originPos[i - 1];
			}
		}
		public override string OnPlayerEnter()
		{
			var player = (Player)map.GetEntity(map.GetMapData()["player_x"], map.GetMapData()["player_y"]);
			player.TakeDamage(1);

			return $"You Tried to dive Strong Monster! Life : {player.Hp}. Never Do that!!!";
		}
		public override EntityEnum GetEntityEnum()
		{
			return EntityEnum.Snake_Head;
		}
	}
}
