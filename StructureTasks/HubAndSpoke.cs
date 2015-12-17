using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureTasks
{
	public class Spoke
	{
		public Spoke(Guid guid, int value, Guid hub_guid)
		{
			this.guid = guid;
			this.value = value;
			this.hub_guid = hub_guid;
		}

		public Guid guid;
		public int value;
		public Guid hub_guid;
	}

	public class Hub
	{
		public Hub(Guid guid, string name)
		{
			this.guid = guid;
			this.name = name;
		}

		public Guid guid;
		public string name;
	}
	public interface IHubber
	{
		void AddHub(Hub hub);

		void SaveOrAddSpoke(Spoke spoke);

		void RemoveSpoke(Guid spoke_guid);

		int SpokeCount(Guid hub_guid);

		IEnumerable<Spoke> Spokes(Guid hub_guid);
	}

	public class StupidHubber : IHubber
	{
		private readonly Dictionary<Guid, Spoke> _spokes = new Dictionary<Guid, Spoke>();
		private readonly Dictionary<Guid, Hub> _hubs = new Dictionary<Guid, Hub>();
		public void AddHub(Hub hub)
		{
			if (!_hubs.ContainsKey(hub.guid))
			{
				_hubs.Add(hub.guid, hub);
			}
		}

		public void SaveOrAddSpoke(Spoke spoke)
		{
			Spoke current_spoke;
			if (_spokes.TryGetValue(spoke.guid, out current_spoke))
			{
				current_spoke.value = spoke.value;
				current_spoke.hub_guid = spoke.hub_guid;
			}
			else
			{
				_spokes.Add(spoke.guid, spoke);
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
