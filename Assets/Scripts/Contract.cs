using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Contract
{
	public string Name;
	public int StartingAmount = 0;
	public int Payout = 0;
	public int BidEndTime = 0;
	public string LowBidder = "";
	public int ReservedBid = 0;

	public bool BiddingEnded
	{
		get
		{
			return BidEndTime < TimeManager.Now;
		}
	}

	public List<Requirement> Requirements = new List<Requirement>();

	public Contract() { }

	public bool Evaluate(Tile[,] tiles)
	{
		foreach (var requirement in Requirements)
		{
			if (!requirement.Pass())
				return false;
		}
		return true;
	}

	public bool Bid(string bidder, int amount)
	{
		bool log = false;

		if (log) Debug.Log("Bid attempt by " + bidder + " for " + amount);
		bool bidsuccessful = false;

		if (amount <= 0)
		{
			if (log) Debug.Log("Bid fail: amount <= 0");
			bidsuccessful = false;
		}
		else if (LowBidder == bidder && amount < ReservedBid)
		{
			if (log) Debug.Log("Bid succeed: updated reserve amount");
			bidsuccessful = true;
		}
		else if (LowBidder != "" && LowBidder != bidder)
		{
			if (ReservedBid > 0 && amount < ReservedBid)
			{
				if (log) Debug.Log("Bid succeed: beat previous bid by " + LowBidder);
				Payout = ReservedBid - 1;
				bidsuccessful = true;
			}
			else
			{
				if (log) Debug.Log("Bid fail: not low enough to beat " + Payout + " from " + LowBidder);
				if (amount < Payout && amount >= ReservedBid)
					Payout = amount;
				bidsuccessful = false;
			}
		}
		else if (LowBidder == "" && amount < Payout)
		{
			if (log) Debug.Log("Bid succeed: no other bids");
			Payout--;
			bidsuccessful = true;
		}

		if (bidsuccessful)
		{
			LowBidder = bidder;
			ReservedBid = amount;
		}

		return bidsuccessful;
	}

	public override string ToString()
	{
		string s = Name + ")\n";
		foreach (var r in Requirements)
		{
			s += r.ToString() + ", ";
		}
		s += "Payout: " + Payout;
		return s;
	}

	public class Requirement
	{
		public RoomManager.RoomType Type;
		public RoomManager.RoomQuality Quality;
		public int Count;

		public Requirement() { }

		public Requirement(RoomManager.RoomType type, RoomManager.RoomQuality quality, int count)
		{
			Type = type;
			Quality = quality;
			Count = count;
		}

		public bool Pass()
		{
			int count = 0;
			foreach(var room in RoomManager.Instance.Rooms)
			{
				if(room.Type == Type && room.Quality >= Quality)
				{
					count++;
				}
			}
			return count >= Count;
		}

		public string Save()
		{
			return Count + ":" + Type + ":" + Quality;
		}

		public static Requirement Load(string s)
		{
			Requirement r = new Requirement();
			var parts = s.Split(':');
			r.Count = int.Parse(parts[0]);
			r.Type = (RoomManager.RoomType)System.Enum.Parse(typeof(RoomManager.RoomType), parts[1]);
			r.Quality = (RoomManager.RoomQuality)System.Enum.Parse(typeof(RoomManager.RoomQuality), parts[2]);
			return r;
		}

		public override string ToString()
		{
			return Count + "x " + Quality + " " + Type;
		}
	}


	public string Save()
	{
		string s = "";
		s += Name + "," + Payout + "," + StartingAmount + "," + BidEndTime + "," + LowBidder + "," + ReservedBid;
		foreach (var r in Requirements)
		{
			s += "," + r.Save();
		}
		return s;
	}

	public static Contract Load(string s)
	{
		string[] split = s.Split(',');
		Contract c = new Contract();
		c.Name = split[0];
		c.Payout = int.Parse(split[1]);
		c.StartingAmount = int.Parse(split[2]);
		c.BidEndTime = int.Parse(split[3]);
		c.LowBidder = split[4];
		c.ReservedBid = int.Parse(split[5]);

		for (int i = 6; i < split.Length; i++)
		{
			c.Requirements.Add(Requirement.Load(split[i]));
		}

		return c;
	}

}
