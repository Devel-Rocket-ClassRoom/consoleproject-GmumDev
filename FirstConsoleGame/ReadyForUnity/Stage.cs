using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstConsoleGame.ReadyForUnity
{
	public abstract class Stage : IStage
	{
		private StageData stageData;
		public StageData StageData { get => stageData; }
		public IMap GetMap(int x, int y)
		{
			throw new NotImplementedException();
		}
	}
}
