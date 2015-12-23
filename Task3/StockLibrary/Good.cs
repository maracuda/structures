using System;

namespace StockLibrary
{
	public class Good
	{
		public Guid ID { get; set; }
		public string Name { get; set; }

		public Good(Guid id, string name)
		{
			ID = id;
			Name = name;
		}
	}
}