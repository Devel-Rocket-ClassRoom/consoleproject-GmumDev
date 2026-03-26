using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstConsoleGame
{
	public abstract class Entity : IEntity, IRenderable
	{
		public char symbol;
		public MyVector pos = new MyVector();
		public static IMapDataModifier map; // too many authority

		private bool isUpdatedOnCurFrame = false;
		public bool IsUpdatedOnCurFrame
		{
			get => isUpdatedOnCurFrame; set => isUpdatedOnCurFrame = value;
		}
		public virtual void Render(char[,] buf)
		{
			buf[pos.y, pos.x] = symbol;
		}
		public virtual void Init(MyVector pos) { this.pos = pos; }
		public virtual void Update() { isUpdatedOnCurFrame = true; }

		// Called when player moved to this Entity. This entity replaced to EmptyEntity. 
		protected Player MoveCapturing()
		{
			var mapdata = map.GetMapData();
			MyVector playerPos = new MyVector(mapdata["player_x"], mapdata["player_y"]);
			var player = (Player)map.GetEntity(playerPos.x, playerPos.y);
			MyVector tomove = pos;
			map.SetNewEntity<EmptyEntity>(tomove);
			map.Swap(playerPos, tomove);
			mapdata["player_x"] = tomove.x;
			mapdata["player_y"] = tomove.y;

			return player;
		}
		public abstract string OnPlayerEnter();
		public abstract EntityEnum GetEntityEnum();
	}
}
