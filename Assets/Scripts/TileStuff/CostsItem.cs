using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Tile))]
public class CostsItem : MonoBehaviour
{
	public PlayerInventory.Item item = PlayerInventory.Item.Metals;
	public int count = 1;

	void PlacedByPlayer()
	{
		PlayerInventory.inventory[item] -= count;
		Log.instance.AddMessage(item + " -" + count);
	}

	void RemovedByPlayer()
	{
		PlayerInventory.inventory[item] += count;
		Log.instance.AddMessage(item + " +" + count);
	}
}
