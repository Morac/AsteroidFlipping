using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UILabel))]
public class DisplayInventory : MonoBehaviour {

	UILabel label;

	void Start()
	{
		label = GetComponent<UILabel>();

		PlayerInventory.ItemChangedCallback += InvetoryUpdated;
		label.text = PlayerInventory.inventory.ToString();
	}

	void OnDestroy()
	{
		PlayerInventory.ItemChangedCallback -= InvetoryUpdated;
	}

	void InvetoryUpdated(PlayerInventory.Item m, int val)
	{
		label.text = PlayerInventory.inventory.ToString();
	}
}
