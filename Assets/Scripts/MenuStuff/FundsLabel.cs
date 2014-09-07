using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UILabel))]
public class FundsLabel : MonoBehaviour
{

	UILabel label;

	void Start()
	{
		label = GetComponent<UILabel>();
		UpdateFunds(PlayerInventory.Funds);
		PlayerInventory.FundsChangedCallback += UpdateFunds;
	}

	void OnDestroy()
	{
		PlayerInventory.FundsChangedCallback -= UpdateFunds;
	}

	void UpdateFunds(int val)
	{
		label.text = GlobalSettings.Currency + val.ToString();
	}
}
