using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstConsoleGame.ReadyForUnity
{
	public interface IStage
	{
		IMap GetMap(int x, int y);
		public StageData StageData { get; }
	}
}
