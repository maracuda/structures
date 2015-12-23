using System;
using NUnit.Framework;
using StockLibrary;

namespace StockLibraryTests
{
	public class StockTests
	{
		private Good[] _goods;
		private Partner[] _partners;
		private Stock _stock;

		[SetUp]
		public void Init()
		{
			_goods = new[]
			{
				new Good(Guid.NewGuid(), "Tomato"), 
				new Good(Guid.NewGuid(), "Potato"), 
				new Good(Guid.NewGuid(), "Cucomber"), 
				new Good(Guid.NewGuid(), "Pepper"), 
			};

			_partners = new[]
			{
				new Partner(Guid.NewGuid(), "OSJC Jones"),
				new Partner(Guid.NewGuid(), "Good tomatos"),
				new Partner(Guid.NewGuid(), "Brand new"), 
				new Partner(Guid.NewGuid(), "Big Balls"), 
			};

			_stock = new Stock();
		}

		[Test]
		public void Buy_EmptyQueue_GoodAdded()
		{
			const decimal cost = 10m;
			const int count = 10;
			const int expectedCount = 10;

			_stock.Buy(_partners[0].ID, _goods[0].ID, cost, count);

			Assert.That(_stock.GetGoodCount(_goods[0].ID), Is.EqualTo(expectedCount));
		}

		[Test]
		public void Buy_NotEmptyQueue_GoodAdded()
		{
			const decimal cost = 10m;
			const int count = 10;
			const int expectedCount = 20;

			_stock.Buy(_partners[0].ID, _goods[0].ID, cost, count);
			_stock.Buy(_partners[0].ID, _goods[0].ID, cost, count);

			Assert.That(_stock.GetGoodCount(_goods[0].ID), Is.EqualTo(expectedCount));
		}

		[Test]
		public void Sell_NotEmptyQueueEqualBuyAndSell_GoodRemoved()
		{
			const decimal cost = 10m;
			const int count = 10;
			const int expectedCount = 0;

			_stock.Buy(_partners[0].ID, _goods[0].ID, cost, count);
			_stock.Sell(_partners[0].ID, _goods[0].ID, cost, count);

			Assert.That(_stock.GetGoodCount(_goods[0].ID), Is.EqualTo(expectedCount));
		}

		[Test]
		public void Sell_NotEmptyQueueNotEqualBuyAndSell_GoodRemoved()
		{
			const decimal cost = 10m;
			const int count1 = 10;
			const int count2 = 6;
			const int expectedCount = 4;

			_stock.Buy(_partners[0].ID, _goods[0].ID, cost, count1);
			_stock.Sell(_partners[0].ID, _goods[0].ID, cost, count2);

			Assert.That(_stock.GetGoodCount(_goods[0].ID), Is.EqualTo(expectedCount));
		}

		[Test]
		public void Sell_EmptyQueue_NegativeCount()
		{
			const decimal cost = 10m;
			const int count = 15;
			const int expectedCount = -15;

			_stock.Sell(_partners[0].ID, _goods[0].ID, cost, count);

			Assert.That(_stock.GetGoodCount(_goods[0].ID), Is.EqualTo(expectedCount));
		}

		[Test]
		public void Sell_NotEmptyQueueSellMore_NegativeCount()
		{
			const decimal cost = 10m;
			const int count1 = 10;
			const int count2 = 25;
			const int expectedCount = -15;

			_stock.Buy(_partners[0].ID, _goods[0].ID, cost, count1);
			_stock.Sell(_partners[0].ID, _goods[0].ID, cost, count2);

			Assert.That(_stock.GetGoodCount(_goods[0].ID), Is.EqualTo(expectedCount));
		}

		[Test]
		public void Buy_NotEmptyQueueBuyMore_NegativeCount()
		{
			const decimal cost = 10m;
			const int count1 = 20;
			const int count2 = 10;
			const int expectedCount = -10;

			_stock.Sell(_partners[0].ID, _goods[0].ID, cost, count1);
			_stock.Buy(_partners[0].ID, _goods[0].ID, cost, count2);

			Assert.That(_stock.GetGoodCount(_goods[0].ID), Is.EqualTo(expectedCount));
		}

		[Test]
		public void FirstCost_TwoBatches_PositiveCount()
		{
			const decimal cost = 10m;
			const int count1 = 20;
			const int count2 = 10;
			const decimal expectedCount = 2/3m;

			_stock.Buy(_partners[0].ID, _goods[0].ID, cost, count1);
			_stock.Buy(_partners[0].ID, _goods[0].ID, cost, count2);

			Assert.That(_stock.FirstCost(_goods[0].ID), Is.EqualTo(expectedCount));
		}

		[Test]
		public void FirstCost_SellNotExisted_Null()
		{
			const decimal cost = 10m;
			const int count1 = 20;
			const int count2 = 10;
			const decimal expectedCount = 0;

			_stock.Sell(_partners[0].ID, _goods[0].ID, cost, count1);
			_stock.Sell(_partners[0].ID, _goods[0].ID, cost, count2);

			Assert.That(_stock.FirstCost(_goods[0].ID), Is.EqualTo(expectedCount));
		}

		[Test]
		public void FirstCost_AfterPositiveBuy_Positive()
		{
			const decimal cost = 10m;
			const int count1 = 10;
			const int count2 = 25;
			const decimal expectedCount = 10/15m;

			_stock.Sell(_partners[0].ID, _goods[0].ID, cost, count1);
			_stock.Buy(_partners[0].ID, _goods[0].ID, cost, count2);

			Assert.That(_stock.FirstCost(_goods[0].ID), Is.EqualTo(expectedCount));
		}

		[Test]
		public void GoodCount_AfterPositiveBuy_Positive()
		{
			const decimal cost = 10m;
			const int count1 = 10;
			const int count2 = 25;
			const int expectedCount = 15;

			_stock.Sell(_partners[0].ID, _goods[0].ID, cost, count1);
			_stock.Buy(_partners[0].ID, _goods[0].ID, cost, count2);

			Assert.That(_stock.GoodCount(_partners[0].ID, _goods[0].ID), Is.EqualTo(expectedCount));
		}

		[Test]
		public void GoodCount_NotExistedPartner_Positive()
		{
			const decimal cost = 10m;
			const int count1 = 10;
			const int count2 = 25;
			const int expectedCount = 0;

			_stock.Sell(_partners[0].ID, _goods[0].ID, cost, count1);
			_stock.Buy(_partners[0].ID, _goods[0].ID, cost, count2);

			Assert.That(_stock.GoodCount(Guid.NewGuid(), _goods[0].ID), Is.EqualTo(expectedCount));
		}

		[Test]
		public void TotalGoodSum_AfterPositiveBuy_Positive()
		{
			const decimal cost = 10m;
			const int count1 = 10;
			const int count2 = 25;
			const decimal expectedCount = 150m;

			_stock.Sell(_partners[0].ID, _goods[0].ID, cost, count1);
			_stock.Buy(_partners[0].ID, _goods[0].ID, cost, count2);

			Assert.That(_stock.TotalGoodSum(_partners[0].ID, _goods[0].ID), Is.EqualTo(expectedCount));
		}

		[Test]
		public void TotalGoodSum_NotExistedPartner_Positive()
		{
			const decimal cost = 10m;
			const int count1 = 10;
			const int count2 = 25;
			const decimal expectedCount = 0;

			_stock.Sell(_partners[0].ID, _goods[0].ID, cost, count1);
			_stock.Buy(_partners[0].ID, _goods[0].ID, cost, count2);

			Assert.That(_stock.TotalGoodSum(Guid.NewGuid(), _goods[0].ID), Is.EqualTo(expectedCount));
		}

		[Test]
		public void TotalGoodSum_NotExistedGood_ThrowsException()
		{
			var result = Assert.Throws<Exception>(() => _stock.TotalGoodSum(Guid.Empty, Guid.Empty)).Message;

			Assert.That(result, Is.EqualTo("Такого товара на складе нет!"));
		}

		[Test]
		public void GoodCount_NotExistedGood_ThrowsException()
		{
			var result = Assert.Throws<Exception>(() => _stock.GoodCount(Guid.Empty, Guid.Empty)).Message;

			Assert.That(result, Is.EqualTo("Такого товара на складе нет!"));
		}

		[Test]
		public void FirstCost_NotExistedGood_ThrowsException()
		{
			var result = Assert.Throws<Exception>(() => _stock.FirstCost(Guid.Empty)).Message;

			Assert.That(result, Is.EqualTo("Такого товара на складе нет!"));
		}
	}
}