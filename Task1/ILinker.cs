using System;
using System.Collections.Generic;

namespace Task1
{
	public interface ILinker
	{
		Guid AddNode(string s);
		void Attach(Guid first, Guid second);
		void Detach(Guid first, Guid second);
		IEnumerable<string> GetNeighbors(Guid id);
	}
}