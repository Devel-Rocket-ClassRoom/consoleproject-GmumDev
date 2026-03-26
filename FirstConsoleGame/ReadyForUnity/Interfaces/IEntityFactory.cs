using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstConsoleGame.ReadyForUnity
{
	public interface IEntityFactory
	{
		IEntity GetNewEntity<T>() where T : IEntity, new();
	}
}
