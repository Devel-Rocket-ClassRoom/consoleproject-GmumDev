using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FirstConsoleGame.Program;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FirstConsoleGame
{
	public class DungeonGame
	{
		private static DungeonGame instance;

		private AlertRenderer alertRenderer;
		private DungeonGame_Renderer renderer;
		private DungeonGame_StageFactory stageManager;
		private DungeonGame_Map curMap;
		private Player player;

		private int curStage = 1;
		public int CurStage { get => curStage; }
		private string msg = "Welcome to my dungeon game! Input your move and defeat every monster, then run out to doors!";

		private bool isRunning = true;

		// -----
		private DungeonGame()
		{
			InitGame();
		}
		public Player GetPlayer()
		{
			return player;
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
			isRunning = true;
			while (isRunning)
			{
				Update();
			}
		}

		// Init New Games
		public void InitGame()
		{
			// init--------
			player = (Player)EntityManager.GetInstance().GetNewInstance<Player>(new MyVector(1, 1));

			stageManager = DungeonGame_StageFactory.GetInstance();

			renderer = DungeonGame_Renderer.GetInstance(stageManager.MaxMapSize);

			InitAlerts();

			// set stage----
			SetNewStage();
		}
		public void InitGameWithRawData(RawGameData data)
		{
			// init--------
			player.InitWithRawData(data.playerData);

			// set stage----
			SetNewStageWithRawData(data);
		}
		private void InitAlerts()
		{
			alertRenderer = AlertRenderer.GetInstance();

			alertRenderer.alertGameoverBox.SetCallback('R', "Restart", RestartGame);
			alertRenderer.alertGameoverBox.SetCallback('Q', "Quit", QuitGame);
			alertRenderer.alertGameoverBox.SetCallback('D', "Donate", alertRenderer.alertDonateBox.Alert);
			alertRenderer.alertGameoverBox.LocalRender();

			alertRenderer.alertDonateBox.SetCallback('Q', "Quit", QuitGame);
			alertRenderer.alertDonateBox.SetCallback(' ', "Thank you", alertRenderer.alertDonateBox.Alert);
			alertRenderer.alertDonateBox.SetCallback('R', "Restart", RestartGame);
			alertRenderer.alertDonateBox.SetCallback('1', "Buy Coffee", RestartGame);
			alertRenderer.alertDonateBox.SetCallback('2', "Buy Lunch", RestartGame);
			alertRenderer.alertDonateBox.SetCallback('3', "Buy Dinner", RestartGame);
			alertRenderer.alertDonateBox.LocalRender();

			alertRenderer.alertSaveLoadBox.SetCallback('Q', "Quit", QuitGame);
			alertRenderer.alertSaveLoadBox.SetCallback('P', "Cancel", () => { });
			alertRenderer.alertSaveLoadBox.SetCallback('R', "Restart", RestartGame);
			alertRenderer.alertSaveLoadBox.SetCallback('S', "Save", SaveGame);
			alertRenderer.alertSaveLoadBox.SetCallback('L', "Load", LoadGame);
			alertRenderer.alertSaveLoadBox.LocalRender();
		}
	
		// Set new Stage to game.
		private void SetNextStage()
		{
			stageManager.SetNewStage(player);
			SetCurMap(stageManager.GetMap(stageManager.StartPos), DirIndex.Right);
			curStage++;

			// no need to initial draw
			renderer.UpdateNewStage();
		}
		private void SetNewStage()
		{
			stageManager.SetNewStage(player);
			SetCurMap(stageManager.GetMap(stageManager.StartPos), DirIndex.Right);
			curStage = 1;

			// initial draw
			renderer.UpdateNewStage();
			renderer.Update(curMap, curStage, msg, player.MaxHp, player.Hp);
			renderer.Draw();
		}
		private void SetNewStageWithRawData(RawGameData data)
		{
			stageManager.SetNewStageWithRawData(data.stageData);
			SetCurMap(stageManager.GetMap(stageManager.StartPos), DirIndex.Right);
			curStage = data.curStage;

			// initial draw
			renderer.UpdateNewStage();
			renderer.Update(curMap, curStage, msg, player.MaxHp, player.Hp);
			renderer.Draw();
		}

		// Restart, Save, Load, Quit Game
		private void RestartGame()
		{
			player.Init(new MyVector(1, 1));
			SetNewStage();
		}
		private void SaveGame()
		{
			Utility.SaveData(RawDataParser.GetRawGameData(this), "Game.json");
		}
		private void LoadGame()
		{
			RawGameData data = Utility.LoadData<RawGameData>("Game.json");
			InitGameWithRawData(data);
		}
		private void QuitGame()
		{
			isRunning = false;
		}

		// Set Current map to game and initialize
		private void SetCurMap(DungeonGame_Map map, DirIndex dir)
		{
			curMap = map;
			curMap.SetPlayerPosOnNewMap(player, dir);
			curMap.UpdateMapData();
			Entity.map = (IMapDataModifier)curMap;
		}

		// Main loop
		private bool Update()
		{
			curMap.Update();

			// --- Manage Input
			char dir = Utility.InputSingleChar();
			if (dir == 'Q')
			{
				alertRenderer.alertSaveLoadBox.Alert();
			}
			else
			{
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
					msg = "Next Stage!!";
					SetNextStage();
				}

				if (player.IsDead)
				{
					alertRenderer.alertGameoverBox.Alert();
				}
			}

			do
			{
				// enqueue callback of alert with user selection
				alertRenderer.DrawAndSetCallbacks();

				// do callbacks
				alertRenderer.DoCallbacks();

				// some alerts can be invoked by other alerts. so check again. 
			} while (alertRenderer.UpdateAlertedCnt() > 0);

			renderer.Update(curMap, curStage, msg, player.MaxHp, player.Hp);
			renderer.Draw();

			
			
			return true;
		}

	}
}
