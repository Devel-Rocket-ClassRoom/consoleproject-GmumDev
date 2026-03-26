using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstConsoleGame.ReadyForUnity
{
	public interface IMapFactory
	{
		// 빈 맵을 반환합니다. 
		IMap GenerateDefaultMap<T>() where T : IMap, new();

		// 맵에 가할 수정의 내용입니다. 
		delegate IMap FillTask(IMap map);
	}
}
