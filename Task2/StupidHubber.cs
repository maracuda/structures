using System;
using System.Collections.Generic;
using System.Linq;

namespace Task2
{
	public class StupidHubber : IHubber
	{
		private readonly Dictionary<Guid, Hub> _hubs = new Dictionary<Guid, Hub>();
		private readonly Dictionary<Guid, Spoke> _spokes = new Dictionary<Guid, Spoke>();

		public void AddHub(Hub hub)
		{
			if (!_hubs.ContainsKey(hub.guid))
			{
				_hubs.Add(hub.guid, hub);
			}
		}

		public void SaveOrAddSpoke(Spoke current)
		{
			Spoke current_spoke;
			if (_spokes.TryGetValue(current.guid, out current_spoke))
			{
				current_spoke.value = current.value;
				current_spoke.hub_guid = current.hub_guid;
			}
			else
			{
				_spokes.Add(current.guid, current);
			}
		}

		public void RemoveSpoke(Guid spoke_guid)
		{
			if (_spokes.ContainsKey(spoke_guid))
			{
				_spokes.Remove(spoke_guid);
			}
		}

		public int SpokeCount(Guid hub_guid)
		{
			return _spokes.Values.Count(k => k.hub_guid == hub_guid);
		}

		public IEnumerable<Spoke> Spokes(Guid hub_guid)
		{
			return _spokes.Values.Where(k => k.hub_guid == hub_guid);
		}
	}
}