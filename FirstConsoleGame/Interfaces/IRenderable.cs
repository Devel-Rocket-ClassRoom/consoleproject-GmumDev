using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstConsoleGame
{
	public interface IRenderable
	{
		public void Render(char[,] buf);
	}
}
