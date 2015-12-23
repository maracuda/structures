using System;

namespace StockLibrary
{
	public class Batch
	{
		public Guid PartnerID { get; set; }
		public decimal Cost { get; set; }
		public int Count { get; set; }

		public Batch(Guid partner_id, decimal cost, int count)
		{
			PartnerID = partner_id;
			Cost = cost;
			Count = count;
		}
	}
}