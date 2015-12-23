using System;
using System.Collections.Generic;

namespace Task2
{
	public interface IHubber
	{
		void AddHub(Hub hub);

		void SaveOrAddSpoke(Spoke current);

		void RemoveSpoke(Guid spoke_guid);

		int SpokeCount(Guid hub_guid);

		IEnumerable<Spoke> Spokes(Guid hub_guid);
	}
}