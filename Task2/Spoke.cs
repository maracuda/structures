using System;

namespace Task2
{
	public class Spoke
	{
		public Guid guid;
		public Guid hub_guid;
		public int value;

		public Spoke(Guid guid, int value, Guid hub_guid)
		{
			this.guid = guid;
			this.value = value;
			this.hub_guid = hub_guid;
		}

		public bool IsSame(Spoke spoke)
		{
			return guid.Equals(spoke.guid) && hub_guid.Equals(spoke.hub_guid) && value.Equals(spoke.value);
		}
	}
}