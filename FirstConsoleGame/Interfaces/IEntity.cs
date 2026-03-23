using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstConsoleGame
{
	public interface IEntity
	{
		void Init(MyVector pos);
		void Update();
		string OnPlayerEnter();
		public bool IsUpdatedOnCurFrame { get; set; }
	}
}
