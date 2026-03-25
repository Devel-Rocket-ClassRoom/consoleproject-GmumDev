using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FirstConsoleGame
{
	public struct RawMapData
	{
		[JsonInclude]
		public int sizex;
		[JsonInclude]
		public int sizey;
		[JsonInclude]
		public int gridx;
		[JsonInclude]
		public int gridy;
		[JsonInclude]
		public bool isStartMap;
		[JsonInclude]
		public bool isEndMap;
		[JsonInclude]
		public int[][] buf;

	}
}
