using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FirstConsoleGame.Program;

namespace FirstConsoleGame
{
	public class DungeonGame
	{
		private static DungeonGame instance;
		private DungeonGame_Renderer renderer;
		private DungeonGame_StageFactory stageManager;
		private DungeonGame_Map curMap;
		private Player player;

		private int curStage = 1;
		private string msg = "Welcome to my dungeon game! Input your move and defeat every monster, then run out to doors!";

		// -----
		private DungeonGame()
		{
			Init();
		}
		public static DungeonGame GetInstance()
		{
			if (instance == null)
			{
				instance = new DungeonGame();
			}

			return instance;
		}
		public void Play()
		{
			bool isRunning = true;
			while (isRunning)
			{
				isRunning = Update();
			}
		}

		// -----
		public void Init()
		{
			player = (Player)EntityManager.GetInstance().GetNewInstance<Player>(new MyVector(1, 1));

			stageManager = DungeonGame_StageFactory.GetInstance();
			renderer = DungeonGame_Renderer.GetInstance(stageManager.MaxMapSize);

			SetNewStage();

			renderer.Draw(curMap, curStage, msg, player.MaxHp, player.Hp);
		}
		private void SetNewStage()
		{
			stageManager.SetNewStage(player);
			SetCurMap(stageManager.GetMap(stageManager.StartPos), DirIndex.Right);
			renderer.UpdateNewStage();
		}
		private void SetCurMap(DungeonGame_Map map, DirIndex dir)
		{
			curMap = map;
			curMap.SetPlayerPosOnNewMap(player, dir);
			curMap.UpdateMapData();
			Entity.map = curMap;
		}
		private bool Update()
		{
			curMap.Update();

			// --- Manage Input
			char dir = Utility.InputSingleChar();
			if (dir == 'Q') return false;

			// --- Get Next Move. 
			MyVector tomove = player.GetMove(dir, curMap.size);
			MyVector deltaMove = new MyVector(tomove.x - player.pos.x, tomove.y - player.pos.y);

			IEntity targetEntity = curMap.GetEntity(tomove.x, tomove.y);
			msg = targetEntity.OnPlayerEnter();

			if (targetEntity is Door && curMap.IsClear())
			{
				curMap.SetNewEntity<EmptyEntity>(player.pos);
				SetCurMap(curMap.GetDoor(Utility.charToDirIndex[dir]).GetTargetMap(), Utility.charToDirIndex[dir]);

				msg = "Next Map!!";
			}
			if (targetEntity is NextStageBicon && stageManager.IsClear())
			{
				SetNewStage();
				msg = "Next Stage!!";
				curStage++;
			}

			renderer.Draw(curMap, curStage, msg, player.MaxHp, player.Hp);

			return true;
		}
	}
}
