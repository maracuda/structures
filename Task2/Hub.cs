using System;

namespace Task2
{
	public class Hub
	{
		public Guid guid;
		public string name;

		public Hub(Guid guid, string name)
		{
			this.guid = guid;
			this.name = name;
		}
	}
}