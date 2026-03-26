using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace FirstConsoleGame
{
	public static class RawDataParser
	{
		private static RawPlayerData GetRawPlayerData(Player player)
		{
			var data = new RawPlayerData();
			data.hp = player.Hp;
			data.money = player.Money;
			data.InventoryCnt = new List<int>();
			data.InventoryItemID = new List<ItemID>();
			foreach(var it in player.Inventory)
			{
				data.InventoryItemID.Add(it.Key);
				data.InventoryCnt.Add(it.Value);
			}
			return data;
		}
		private static RawStageData GetRawStageData()
		{
			var data = new RawStageData();
			data.mapdatas = new List<RawMapData>();
			
			
			
			
			StageFactory factory = StageFactory.GetInstance();
			for (int i = 0; i < StageFactory.GRID_HEIGHT; i++)
			{
				for (int j = 0; j < StageFactory.GRID_WIDTH; j++)
				{
					var map = factory.GetMap(new MyVector(j, i));
					if(map != null)
					{
						var mapdata = GetRawMapData(map);
						data.mapdatas.Add(mapdata);
					}
				}
			}

			return data;
		}
		public static RawGameData GetRawGameData(DungeonGame game)
		{
			var data = new RawGameData();
			data.curStage = game.CurStage;
			data.stageData = GetRawStageData();
			data.playerData = GetRawPlayerData(game.GetPlayer());

			return data;
		}

		private static RawMapData GetRawMapData(Map map)
		{
			var data = new RawMapData();
			data.sizex = map.size.x;
			data.sizey = map.size.y;
			data.gridx = map.GridPos.x;
			data.gridy = map.GridPos.y;
			data.buf = new int[map.size.y][];
			data.isEndMap = map.isEndMap;
			data.isStartMap = map.isStartMap;
			for (int i = 0; i < map.size.y; i++)
			{
				data.buf[i] = new int[map.size.x];
				for (int j = 0; j < map.size.x; j++)
				{
					EntityEnum entity = map.GetEntity(j, i).GetEntityEnum();
					switch(entity)
					{
						case EntityEnum.Door:
						case EntityEnum.Player:
						case EntityEnum.ItemShop:
						case EntityEnum.Snake_Tail:
							data.buf[i][j] = (int)EntityEnum.EmptyEntity;
							break;
						default:
							data.buf[i][j] = (int)entity;
							break;
					}
				}
			}
			return data;
		}
		public static Map GetMapFromRawData(RawMapData data)
		{
			var map = MapFactory.GetInstance().GetEmptyMap(new MyVector(data.sizex, data.sizey), new MyVector(data.gridx, data.gridy));

			for (int i = 0; i < data.sizey; i++)
			{
				for (int j = 0; j < data.sizex; j++)
				{
					switch (data.buf[i][j])
					{
						case (int)EntityEnum.Fence:
							map.SetNewEntity<Fence>(new MyVector(j, i));
							break;
						case (int)EntityEnum.EmptyEntity:
							map.SetNewEntity<EmptyEntity>(new MyVector(j, i));
							break;
						case (int)EntityEnum.Monster:
							map.SetNewEntity<Monster>(new MyVector(j, i));
							break;
						case (int)EntityEnum.BigSnake_Head:
							map.SetNewEntity<BigSnake_Head>(new MyVector(j, i));
							break;
						case (int)EntityEnum.HealingPotion:
							map.SetNewEntity<HealingPotion>(new MyVector(j, i));
							break;
						case (int)EntityEnum.MapClearBicon:
							map.SetNewEntity<MapClearBicon>(new MyVector(j, i));
							break;
						case (int)EntityEnum.NextStageBicon:
							map.SetNewEntity<NextStageBicon>(new MyVector(j, i));
							break;
						case (int)EntityEnum.Snake_Head:
							map.SetNewEntity<Snake_Head>(new MyVector(j, i));
							break;
						case (int)EntityEnum.SnakeBaby:
							map.SetNewEntity<SnakeBaby>(new MyVector(j, i));
							break;
						default:
							throw new Exception("Whats this Entity? Check Saved data or save logic. 이건 무슨 엔티티애요 저장은 안하고 불러오려고 하고 잇잔아요");
							break;
					}
				}
			}
			map.isStartMap = data.isStartMap;
			map.isEndMap = data.isEndMap;
			// 맵의 클리어 정보를 저장하고 불러오는거 구현해야됨. 
			map.IsClear = () => true;
			return map;
		}



	}
}
