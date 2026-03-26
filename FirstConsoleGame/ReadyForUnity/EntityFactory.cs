using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstConsoleGame.ReadyForUnity
{
	public class EntityFactory : IEntityFactory
	{
		public IEntity GetNewEntity<T>() where T : IEntity, new()
		{
			T entity = new T();
			return entity;
		}
	}
}
