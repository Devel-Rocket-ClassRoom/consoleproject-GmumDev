using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstConsoleGame.ReadyForUnity
{
	public interface IMap
	{
		// 새로운 엔티티를 생성하여 배치합니다. 성공시 True를 반환
		void SetNewEntity<T>(int x, int y) where T : IEntity, new();

		// 기존의 엔티티를 배치합니다. 성공시 True를 반환
		bool SetEntity(IEntity entity, int x, int y);

		// 엔티티를 반환합니다. 
		IEntity GetEntity(int x, int y);

		public MapData MapData { get; }
	}
}
