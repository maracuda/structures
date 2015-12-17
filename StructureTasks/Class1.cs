using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureTasks
{
	public interface ILinker
	{
		Guid AddNode(string s);
		void Attach(Guid first, Guid second);
		void Detach(Guid first, Guid second);
		IEnumerable<string> GetNeighbors(Guid id);
	}

	public class SimpleLinker : ILinker
	{
		private readonly Dictionary<Guid, string> _nodes = new Dictionary<Guid, string>();
		private readonly Dictionary<Guid, DateTime> _links = new Dictionary<Guid, DateTime>();
		private readonly Dictionary<Guid, Dictionary<Guid, Guid>> _linksTable = new Dictionary<Guid, Dictionary<Guid, Guid>>();
		public Guid AddNode(string s)
		{
			var new_guid = Guid.NewGuid();
			_nodes.Add(new_guid, s);

			return new_guid;
		}

		public void Attach(Guid first, Guid second)
		{
			if (!(_nodes.ContainsKey(first) && _nodes.ContainsKey(second)))
			{
				throw new Exception("Одного из ключей не существует");
			}


			if (_linksTable.ContainsKey(first) && _linksTable[first].ContainsKey(second))
			{
				throw new Exception("Такая связка уже существует");
			}

			var new_guid = Guid.NewGuid();
			AddNeighbor(first, second, new_guid);
			AddNeighbor(second, first, new_guid);

			_links.Add(new_guid, DateTime.Now);
		}

		public void Detach(Guid first, Guid second)
		{
			if (!(_nodes.ContainsKey(first) && _nodes.ContainsKey(second)))
			{
				throw new Exception("Одного из ключей не существует");
			}

			if (!(_linksTable.ContainsKey(first) && _linksTable[first].ContainsKey(second)))
			{
				throw new Exception("Такой связки не существует");
			}

			_links.Remove(_linksTable[first][second]);
			_linksTable[first].Remove(second);
			_linksTable[second].Remove(first);
		}

		public IEnumerable<string> GetNeighbors(Guid id)
		{
			return _linksTable[id].Keys.Select(k => _nodes[k]);
		}

		private void AddNeighbor(Guid n1, Guid n2, Guid link_id)
		{
			Dictionary<Guid, Guid> dict;
			if (_linksTable.TryGetValue(n1, out dict))
			{
				dict.Add(n2, link_id);
			}
			else
			{
				dict = new Dictionary<Guid, Guid> { { n2, link_id } };
				_linksTable.Add(n1, dict);
			}
		}
	}
}
