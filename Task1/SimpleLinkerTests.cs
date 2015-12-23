using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Task1;

namespace StructureTasks
{
	class SimpleLinkerTests
	{
		private SimpleLinker _simpleLinker;
		[SetUp]
		public void Init()
		{
			_simpleLinker = new SimpleLinker();
		}

		[Test()]
		public void Attach_NotExistedNodes_ThrowsException()
		{
			var result = Assert.Throws<Exception>(() => _simpleLinker.Attach(Guid.NewGuid(), Guid.NewGuid())).Message;

			Assert.That(result, Is.EqualTo("Одного из ключей не существует"));
		}

		[Test()]
		public void Attach_TwoExistedNodes_GetNeighbor()
		{
			var id1 = _simpleLinker.AddNode("node 1");
			var id2 = _simpleLinker.AddNode("node 2");
			const string expected = "node 2";

			_simpleLinker.Attach(id1, id2);

			Assert.That(_simpleLinker.GetNeighbors(id1).First(), Is.EqualTo(expected));
		}

		[Test()]
		public void Attach_TrheeExistedNodes_GetNeighbor()
		{
			var id1 = _simpleLinker.AddNode("node 1");
			var id2 = _simpleLinker.AddNode("node 2");
			var id3 = _simpleLinker.AddNode("node 3");
			var expected1 = new[] { "node 2", "node 3" };
			var expected2 = new[] { "node 1" };


			_simpleLinker.Attach(id1, id2);
			_simpleLinker.Attach(id1, id3);

			Assert.That(_simpleLinker.GetNeighbors(id1).ToArray(), Is.EqualTo(expected1));
			Assert.That(_simpleLinker.GetNeighbors(id2).ToArray(), Is.EqualTo(expected2));
			Assert.That(_simpleLinker.GetNeighbors(id3).ToArray(), Is.EqualTo(expected2));
		}

		[Test()]
		public void Attach_ExistedLink_ThrowsException()
		{
			var id1 = _simpleLinker.AddNode("node 1");
			var id2 = _simpleLinker.AddNode("node 2");

			_simpleLinker.Attach(id1, id2);

			var result = Assert.Throws<Exception>(() => _simpleLinker.Attach(id1, id2)).Message;

			Assert.That(result, Is.EqualTo("Такая связка уже существует"));
		}

		[Test()]
		public void Attach_NotExistedNeighbor_ThrowsException()
		{
			var id1 = _simpleLinker.AddNode("node 1");
			var id2 = Guid.NewGuid();

			var result = Assert.Throws<Exception>(() => _simpleLinker.Attach(id1, id2)).Message;

			Assert.That(result, Is.EqualTo("Одного из ключей не существует"));
		}

		[Test()]
		public void Detach_NotExistedNeighbor_ThrowsException()
		{
			var id1 = _simpleLinker.AddNode("node 1");
			var id2 = Guid.NewGuid();

			var result = Assert.Throws<Exception>(() => _simpleLinker.Detach(id1, id2)).Message;

			Assert.That(result, Is.EqualTo("Одного из ключей не существует"));
		}

		[Test()]
		public void Detach_NotExistedLink_ThrowsException()
		{
			var id1 = _simpleLinker.AddNode("node 1");
			var id2 = _simpleLinker.AddNode("node 2");

			var result1 = Assert.Throws<Exception>(() => _simpleLinker.Detach(id1, id2)).Message;
			var result2 = Assert.Throws<Exception>(() => _simpleLinker.Detach(id2, id1)).Message;

			Assert.That(result1, Is.EqualTo("Такой связки не существует"));
			Assert.That(result2, Is.EqualTo("Такой связки не существует"));
		}

		[Test()]
		public void Detach_TwoExistedNeighbors_LinkRemoved()
		{
			var id1 = _simpleLinker.AddNode("node 1");
			var id2 = _simpleLinker.AddNode("node 2");

			_simpleLinker.Attach(id1, id2);
			_simpleLinker.Detach(id1, id2);

			var result1 = Assert.Throws<Exception>(() => _simpleLinker.Detach(id1, id2)).Message;
			var result2 = Assert.Throws<Exception>(() => _simpleLinker.Detach(id2, id1)).Message;

			Assert.That(result1, Is.EqualTo("Такой связки не существует"));
			Assert.That(result2, Is.EqualTo("Такой связки не существует"));
			Assert.That(_simpleLinker.GetNeighbors(id1).ToArray().Length, Is.EqualTo(0));
		}

		[Test()]
		public void Detach_ThreeExistedNodesOneLinkRemoved_GetOneLink()
		{
			var id1 = _simpleLinker.AddNode("node 1");
			var id2 = _simpleLinker.AddNode("node 2");
			var id3 = _simpleLinker.AddNode("node 3");
			var expected1 = new[] { "node 2" };
			var expected2 = new[] { "node 1" };


			_simpleLinker.Attach(id1, id2);
			_simpleLinker.Attach(id1, id3);
			_simpleLinker.Detach(id1, id3);

			Assert.That(_simpleLinker.GetNeighbors(id1).ToArray(), Is.EqualTo(expected1));
			Assert.That(_simpleLinker.GetNeighbors(id2).ToArray(), Is.EqualTo(expected2));
		}
	}
}
