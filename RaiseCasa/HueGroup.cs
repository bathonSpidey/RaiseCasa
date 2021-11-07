using System.Collections.Generic;

namespace RaiseCasa
{
	public class HueGroup
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public List<int> Lights { get; set; }
		public State Action { get; set; }
	}
}