using UnityEngine;
using System.Collections;

public class BlueprintCard : MonoBehaviour {

	public GameObject PreviewPos;
	public UILabel NameLabel;
	public UILabel UnlockCostLabel;

	public UILabel BuildCostLabel;
	public UILabel EffectLabel; //eg. add $100 to value, supply oxygen, etc.

	public UILabel DescriptionLabel;

	Tile tile;

	public void Init(Tile init)
	{
		tile = init;
		NameLabel.text = tile.name;
		UnlockCostLabel.text = GlobalSettings.Currency + tile.BuyBlueprintCost.ToString();
		if(PlayerInventory.CanAfford(tile.BuyBlueprintCost))
		{
			UnlockCostLabel.color = Color.green;
		}
		else
		{
			UnlockCostLabel.color = Color.red;
		}

		BuildCostLabel.text = tile.CostString();
		DescriptionLabel.text = tile.Description;

		var valuecomp = tile.GetComponent<Valuable>();
		if (valuecomp)
		{
			EffectLabel.text = "Value +" + GlobalSettings.Currency + valuecomp.value;
		}
		else
		{
			EffectLabel.text = "Decorational";
		}


		//instantiate tile
		//put under preview pos
		//localscale reset
		//UI layer
		Tile inst = Instantiate(tile, PreviewPos.transform.position, PreviewPos.transform.rotation) as Tile;

		int uilayer = LayerMask.NameToLayer("UI");
		inst.gameObject.layer = uilayer;
		foreach (var t in inst.GetComponentsInChildren<Transform>())
			t.gameObject.layer = uilayer;

		inst.transform.parent = PreviewPos.transform;
		inst.transform.localScale = Vector3.one;

	}

	void Start()
	{
		PlayerInventory.FundsChangedCallback += CurrencyUpdated;
	}

	void OnDestroy()
	{
		PlayerInventory.FundsChangedCallback -= CurrencyUpdated;
	}

	void CurrencyUpdated(int newval)
	{
		var valuecomp = tile.GetComponent<Valuable>();
		if (valuecomp)
		{
			EffectLabel.text = "Value +" + GlobalSettings.Currency + valuecomp.value;
		}
		else
		{
			EffectLabel.text = "Decorational";
		}
	}

	public void Buy()
	{
		if(PlayerInventory.CanAfford(tile.BuyBlueprintCost))
		{
			PlayerInventory.Funds -= tile.BuyBlueprintCost;
			PlayerInventory.Unlock(tile);
			Destroy(gameObject);
		}
	}
}
