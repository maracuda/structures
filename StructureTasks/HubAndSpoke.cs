using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

namespace StructureTasks
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

    public interface IHubber
    {
        void AddHub(Hub hub);

        void SaveOrAddSpoke(Spoke current);

        void RemoveSpoke(Guid spoke_guid);

        int SpokeCount(Guid hub_guid);

        IEnumerable<Spoke> Spokes(Guid hub_guid);
    }

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

    public class NotSoStupidHubber : IHubber
    {
        private readonly Dictionary<Guid, List<Guid>> _hubChilds = new Dictionary<Guid, List<Guid>>();
        private readonly Dictionary<Guid, Hub> _hubs = new Dictionary<Guid, Hub>();
        private readonly Dictionary<Guid, Spoke> _spokes = new Dictionary<Guid, Spoke> ();

        public void AddHub(Hub hub)
        {
            if (!_hubs.ContainsKey(hub.guid))
            {
                _hubs.Add(hub.guid, hub);
            }
        }

        public void SaveOrAddSpoke(Spoke current)
        {
            Spoke existed;
            if (_spokes.TryGetValue(current.guid, out existed))
            {
                if (existed.IsSame(current)) return;

                if (existed.hub_guid != current.hub_guid)
                {
                    if (existed.hub_guid != Guid.Empty)
                    {
                        _hubChilds[existed.hub_guid].Remove(existed.guid);
                    }
                    AddHubChild(current);
                }
                _spokes[existed.guid] = current;
            }
            else
            {
                _spokes.Add(current.guid, current);
                AddHubChild(current);
            }
        }

        private void AddHubChild(Spoke spoke)
        {
            if (spoke.hub_guid == Guid.Empty) return;

            if (_hubChilds.ContainsKey(spoke.hub_guid))
            {
                _hubChilds[spoke.hub_guid].Add(spoke.guid);
            }
            else
            {
                _hubChilds.Add(spoke.hub_guid, new List<Guid> { spoke.guid });
            }
        }

        public void RemoveSpoke(Guid spoke_guid)
        {
            Spoke existed;
            if (!_spokes.TryGetValue(spoke_guid, out existed)) return;

            _spokes.Remove(existed.guid);
            if (existed.hub_guid != Guid.Empty)
            {
                _hubChilds[existed.hub_guid].Remove(existed.guid);
            }
        }

        public int SpokeCount(Guid hub_guid)
        {
            return _hubChilds[hub_guid].Count;
        }

        public IEnumerable<Spoke> Spokes(Guid hub_guid)
        {
            return _spokes.Where(k => _hubChilds[hub_guid].Contains(k.Key)).Select(k => k.Value);
        }
    }
}