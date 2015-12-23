using System;
using System.Collections.Generic;
using System.Linq;

namespace StockLibrary
{
	public class Stock
	{
		private readonly Dictionary<Guid, Queue<Batch>> _goods;

		public Stock()
		{
			_goods = new Dictionary<Guid, Queue<Batch>>();
		}

		private bool IsBalanceNegative(Guid good_id)
		{
			return _goods[good_id].Count == 1 && _goods[good_id].Peek().Count < 0;
		}

		public void Buy(Guid partner_id, Guid good_id, decimal cost, int count)
		{
			if (_goods.ContainsKey(good_id))
			{
				var good_queue = _goods[good_id];

				if (IsBalanceNegative(good_id))
				{
					if (-good_queue.Peek().Count > count)
					{
						good_queue.Peek().Count += count;
						return;
					}
					count += good_queue.Dequeue().Count;
				}
				good_queue.Enqueue(new Batch(partner_id, cost, count));
			}
			else
			{
				_goods.Add(good_id, new Queue<Batch>(new[] {new Batch(partner_id, cost, count)}));
			}
		}

		public void Sell(Guid partner_id, Guid good_id, decimal cost, int count)
		{
			if (_goods.ContainsKey(good_id))
			{
				var good_queue = _goods[good_id];

				while (good_queue.Count > 0 && cost > 0)
				{
					if (good_queue.Peek().Count <= count)
					{
						count -= good_queue.Dequeue().Count;
					}
					else
					{
						good_queue.Peek().Count -= count;
						return;
					}
				}

				if (count == 0) return;
				if (good_queue.Count == 0)
				{
					good_queue.Enqueue(new Batch(Guid.Empty, 0, -count));
				}
				else
				{
					good_queue.Peek().Count -= count;
				}
			}
			else
			{
				_goods.Add(good_id, new Queue<Batch>(new[] {new Batch(partner_id, 0, -count)}));
			}
		}

		public int GoodCount(Guid partner_id, Guid good_id)
		{
			Queue<Batch> good_queue;
			if (!_goods.TryGetValue(good_id, out good_queue))
			{
				throw new Exception("Такого товара на складе нет!");
			}

			return good_queue.Where(t => t.PartnerID == partner_id).Aggregate(0, (i, batch) => i + batch.Count);
		}

		public decimal TotalGoodSum(Guid partner_id, Guid good_id)
		{
			Queue<Batch> good_queue;
			if (!_goods.TryGetValue(good_id, out good_queue))
			{
				throw new Exception("Такого товара на складе нет!");
			}

			return good_queue.Where(t => t.PartnerID == partner_id).Aggregate(0m, (i, batch) => i + batch.Cost*batch.Count);
		}

		public decimal FirstCost(Guid good_id)
		{
			Queue<Batch> good_queue;
			if (!_goods.TryGetValue(good_id, out good_queue))
			{
				throw new Exception("Такого товара на складе нет!");
			}

			var total_cost = good_queue.Sum(t => t.Cost);
			var total_count = good_queue.Sum(t => t.Count);

			return IsBalanceNegative(good_id) ? 0 : total_cost / total_count;
		}

		public int GetGoodCount(Guid good_id)
		{
			return _goods[good_id].Aggregate(0, (i, batch) => i + batch.Count);
		}
	}
}