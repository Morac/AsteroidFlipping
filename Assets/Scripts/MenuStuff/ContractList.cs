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

	protected override void OnListItemCreate(ListItem item)
	{
		var data = item.Data as Contract;
		item.Labels[(int)Labels.Name].text = data.Type.ToString();


		item.GameObjects[(int)Groups.Bidding].SetActive(true);
		item.GameObjects[(int)Groups.Completion].SetActive(false);
	}
}
