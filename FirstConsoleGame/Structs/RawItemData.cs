using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FirstConsoleGame
{
	public struct RawItemData
	{
		[JsonInclude]
		public string name;
		[JsonInclude]
		public char symbol;
		[JsonInclude]
		public int price;
		[JsonInclude]
		public string descript;
	}
}
