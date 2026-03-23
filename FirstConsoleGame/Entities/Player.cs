using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FirstConsoleGame.Program;

namespace FirstConsoleGame
{
	public class Player : Entity
	{
		private int maxHp;
		private int hp;
		public int MaxHp { get => maxHp; }
		public int Hp { get => hp; }
		private bool isDead = false;
		public bool IsDead { get => isDead; }
		public override void Init(MyVector pos)
		{
			base.Init(pos);
			symbol = 'P';
			maxHp = 8;
			hp = 3;
			isDead = false;
		}
		public void TakeDamage(int damage)
		{
			hp -= damage;
			if (hp > maxHp)
				hp = maxHp;
			if (hp <= 0)
				Die();
		}
		private void Die()
		{
			isDead = true;
		}
		public MyVector GetMove(char dir, MyVector mapSize)
		{
			MyVector tomove = new MyVector(pos.x + Utility.charToDeltaPos[dir].x, pos.y + Utility.charToDeltaPos[dir].y);
			if (mapSize.IsOutOfSquare(tomove))
			{
				tomove = pos;
			}

			return tomove;
		}
		public override string OnPlayerEnter()
		{
			return "Fail to move: out of map";
		}
	}
}
