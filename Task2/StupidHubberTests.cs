﻿using System;
using NUnit.Framework;

namespace Task2
{
	class StupidHubberTests
	{
		private StupidHubber _stupidHubber;

		[SetUp]
		public void Init()
		{
			_stupidHubber = new StupidHubber();
		}

		[Test]
		public void SpokeCount_TwoSpokes_Returns2()
		{
			var hub_guid = Guid.NewGuid();
			var spoke1_guid = Guid.NewGuid();
			var spoke2_guid = Guid.NewGuid();
			_stupidHubber.AddHub(new Hub(hub_guid, "main hub"));
			_stupidHubber.SaveOrAddSpoke(new Spoke(spoke1_guid, 1, hub_guid));
			_stupidHubber.SaveOrAddSpoke(new Spoke(spoke2_guid, 2, hub_guid));

			var count = _stupidHubber.SpokeCount(hub_guid);

			Assert.That(count, Is.EqualTo(2));
		}

		[Test]
		public void SpokeCount_OneSpokeWithLink_Returns1()
		{
			var hub_guid = Guid.NewGuid();
			var spoke1_guid = Guid.NewGuid();
			var spoke2_guid = Guid.NewGuid();
			_stupidHubber.AddHub(new Hub(hub_guid, "main hub"));
			_stupidHubber.SaveOrAddSpoke(new Spoke(spoke1_guid, 1, hub_guid));
			_stupidHubber.SaveOrAddSpoke(new Spoke(spoke2_guid, 2, Guid.Empty));

			var count = _stupidHubber.SpokeCount(hub_guid);

			Assert.That(count, Is.EqualTo(1));
		}
		[Test]
		public void SaveOrAddSpoke_UpdateSecond_SpokeCountReturns2()
		{
			var hub_guid = Guid.NewGuid();
			var spoke1_guid = Guid.NewGuid();
			var spoke2_guid = Guid.NewGuid();
			_stupidHubber.AddHub(new Hub(hub_guid, "main hub"));
			_stupidHubber.SaveOrAddSpoke(new Spoke(spoke1_guid, 1, hub_guid));
			_stupidHubber.SaveOrAddSpoke(new Spoke(spoke2_guid, 2, Guid.Empty));
			_stupidHubber.SaveOrAddSpoke(new Spoke(spoke2_guid, 2, hub_guid));

			var count = _stupidHubber.SpokeCount(hub_guid);

			Assert.That(count, Is.EqualTo(2));
		}

		[Test]
		public void SaveOrAddSpoke_TwoHubs_SpokeCountReturns2()
		{
			var hub_guid1 = Guid.NewGuid();
			var hub_guid2 = Guid.NewGuid();
			var spoke1_guid = Guid.NewGuid();
			var spoke2_guid = Guid.NewGuid();
			var spoke3_guid = Guid.NewGuid();
			_stupidHubber.AddHub(new Hub(hub_guid1, "main hub"));
			_stupidHubber.SaveOrAddSpoke(new Spoke(spoke1_guid, 1, hub_guid1));
			_stupidHubber.SaveOrAddSpoke(new Spoke(spoke2_guid, 2, Guid.Empty));
			_stupidHubber.SaveOrAddSpoke(new Spoke(spoke2_guid, 2, hub_guid2));
			_stupidHubber.SaveOrAddSpoke(new Spoke(spoke3_guid, 2, hub_guid2));

			var count = _stupidHubber.SpokeCount(hub_guid2);

			Assert.That(count, Is.EqualTo(2));
		}
	}
}
