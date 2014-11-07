using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class ContractList : GenericUIList
{
	public GameObject TravelScreen;
	public GameObject TravelScreenCloseButton;
	public GameObject ContractScreen;


	public enum Labels
	{
		Name,
		Description,
		LowBid,
		AutoBid,
		CurrentBid,
		TimeLeft,
		Payout,
		BiddingField
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

	protected override int SortMethod(Transform a, Transform b)
	{
		var c1 = a.GetComponent<ListItem>().Data as Contract;
		var c2 = b.GetComponent<ListItem>().Data as Contract;
		return c1.BidEndTime > c2.BidEndTime ? 1 : -1;
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
		RefreshInfo(item);
		item.Button0Clicked = BidButtonClicked;
		item.Button1Clicked = CompleteButtonClicked;
		GetComponent<UIGrid>().Reposition();
	}

	void RefreshInfo(ListItem item)
	{
		var data = item.Data as Contract;
		item.Labels[(int)Labels.Name].text = data.Name.ToString();

		string requirements = "";
		foreach (var r in data.Requirements)
		{
			requirements += r.ToString() + ", ";
		}

		requirements = requirements.Substring(0, requirements.Length - 2);

		item.Labels[(int)Labels.Description].text = requirements;
		item.Labels[(int)Labels.LowBid].text = Bidtext(data.LowBidder, data.Payout);
		item.Labels[(int)Labels.TimeLeft].text = Timeremaining(data.BidEndTime - TimeManager.Now);

		string reservedtext = "";
		if (data.LowBidder == "You")
		{
			reservedtext = "Reserved bid - " + GlobalSettings.Currency + data.ReservedBid;
		}
		item.Labels[(int)Labels.AutoBid].text = reservedtext;

		item.Labels[(int)Labels.Payout].text = GlobalSettings.Currency + data.Payout.ToString();


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
		if (h > 0)
		{
			s += h + "h ";
			t %= (60 * 60);
		}

		int m = t / 60;
		if (m > 0)
		{
			s += m + "m ";
			t %= 60;
		}

		s += t + "s";

		return s;
	}


	void Update()
	{
		foreach (var item in ListItems)
		{
			if (item == null)
				continue;
			var data = item.Data as Contract;
			if (data.BiddingEnded)
			{
				if (data.LowBidder == "You" && item.GameObjects[(int)Groups.Bidding].activeSelf)
				{
					item.GameObjects[(int)Groups.Bidding].SetActive(false);
					item.GameObjects[(int)Groups.Completion].SetActive(true);
				}
				else if (data.LowBidder != "You")
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

	void BidButtonClicked(ListItem item)
	{
		var contract = item.Data as Contract;

		string bidamtstr = item.Labels[(int)Labels.BiddingField].text;
		int bidamount = int.Parse(bidamtstr);


		if (contract.Bid("You", bidamount))
		{
			RefreshInfo(item);
		}
		else
		{
			item.Labels[(int)Labels.BiddingField].text = "Bid unsuccessful";
		}
	}

	void CompleteButtonClicked(ListItem item)
	{
		var contract = item.Data as Contract;

		if (contract.Evaluate(GameManager.Instance.tileGrid.grid))
		{
			PlayerInventory.Funds += contract.Payout;
			ContractManager.Contracts.Remove(contract);
			GameManager.Instance.Sold = true;
			ContractScreen.SetActive(false);
			TravelScreen.SetActive(true);
			TravelScreenCloseButton.SetActive(false);
		}
	}
}
