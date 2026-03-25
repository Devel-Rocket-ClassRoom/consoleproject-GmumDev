using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FirstConsoleGame
{
	public struct RawStageData
	{
		[JsonInclude]
		public List<RawMapData> mapdatas;
	}
}
