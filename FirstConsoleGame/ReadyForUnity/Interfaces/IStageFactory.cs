using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FirstConsoleGame.ReadyForUnity.IMapFactory;

namespace FirstConsoleGame.ReadyForUnity
{
	public interface IStageFactory
	{
		// 새 스테이지를 생성하고 반환합니다. 
		IStage GenerateStage<T>() where T : IStage, new();

		// 맵에 수정을 가해 반환합니다. 
		IMap FillMap(IMap map, FillTask fillTask);

	}
}
