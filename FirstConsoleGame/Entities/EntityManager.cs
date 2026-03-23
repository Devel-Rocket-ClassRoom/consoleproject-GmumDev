using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstConsoleGame
{
	public class EntityManager
	{
		private static EntityManager instance;
		private EntityManager() { }
		public static EntityManager GetInstance()
		{
			if (instance == null)
				instance = new EntityManager();
			return instance;
		}

		public Entity GetNewInstance<T>(MyVector pos) where T : Entity, new()
		{
			T entity = new T();
			entity.Init(pos);
			return entity;
		}
	}
}
