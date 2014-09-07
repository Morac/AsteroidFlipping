using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Tile))]
public class DropItem : MonoBehaviour
{
	public PlayerInventory.Item item = PlayerInventory.Item.Metals;

	public int MinDropAmount = 0;
	public int MaxDropAmount = 1;

	void RemovedByPlayer()
	{
		int amount = Random.Range(MinDropAmount, MaxDropAmount + 1);
		Log.instance.AddMessage(item + " +" + amount);
		PlayerInventory.inventory[item] += amount;
	}
}
