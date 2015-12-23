using System;

namespace StockLibrary
{
	public class Partner
	{
		public Guid ID { get; set; }
		public string Name { get; set; }

		public Partner(Guid id, string name)
		{
			ID = id;
			Name = name;
		}
	}
}