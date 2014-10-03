using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class ContractList : GenericUIList {

	public enum Labels
	{
		Name,
		Description,
		LowBid,
		AutoBid,
		CurrentBid,
		TimeLeft,
		Payout
	}

	public override List<string> LabelNames()
	{
		return System.Enum.GetNames(typeof(Labels)).ToList();
	}

	public enum Groups
	{
		Bidding,
		Completion
	}

	public override List<string> GroupNames()
	{
		return System.Enum.GetNames(typeof(Groups)).ToList();
	}

	
	protected override System.Collections.Generic.List<object> ListData()
	{
		return ContractManager.Contracts.Cast<object>().ToList();
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		ContractManager.OnContractCreated += AddListItem;
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		ContractManager.OnContractCreated -= AddListItem;
	}

	protected override void OnListItemCreate(ListItem item)
	{
		var data = item.Data as Contract;
		item.Labels[(int)Labels.Name].text = data.Type.ToString();

		string requirements = "";
		foreach(var r in data.Requirements)
		{
			requirements += r.ToString() + "\n";
		}
		item.Labels[(int)Labels.Description].text = requirements;
		item.Labels[(int)Labels.LowBid].text = Bidtext(data.LowBidder, data.Payout);
		item.Labels[(int)Labels.TimeLeft].text = Timeremaining(data.BidEndTime - TimeManager.Now);

		item.GameObjects[(int)Groups.Bidding].SetActive(!data.BiddingEnded);
		item.GameObjects[(int)Groups.Completion].SetActive(data.BiddingEnded);
	}

	string Bidtext(string bidder, int payout)
	{
		if (bidder == "")
			return "Starting bid\n" + GlobalSettings.Currency + payout;
		else
			return "Lowest bid\n" + bidder + " - " + GlobalSettings.Currency + payout;
	}

	string Timeremaining(int time)
	{
		int t = time;
		string s = "Bidding ends in ";
		
		int h = t / (60 * 60);
		if(h > 0)
		{
			s += h + "h ";
			t %= (60 * 60);
		}

		int m = t / 60;
		if(m > 0)
		{
			s += m + "m ";
			t %= 60;
		}

		s += t + "s";

		return s;
	}


	void Update()
	{
		foreach(var item in ListItems)
		{
			if (item == null)
				continue;
			var data = item.Data as Contract;
			if(data.BiddingEnded)
			{
				if(data.LowBidder == "You" && item.GameObjects[(int)Groups.Bidding].activeSelf)
				{
					item.GameObjects[(int)Groups.Bidding].SetActive(false);
					item.GameObjects[(int)Groups.Completion].SetActive(true);
				}
				else
				{
					ContractManager.Contracts.Remove(data);
					Destroy(item.gameObject);
				}
			}
			else
			{
				item.Labels[(int)Labels.LowBid].text = Bidtext(data.LowBidder, data.Payout);
				item.Labels[(int)Labels.TimeLeft].text = Timeremaining(data.BidEndTime - TimeManager.Now);
			}
		}

		ListItems.RemoveAll(item => item == null);
	}
}
