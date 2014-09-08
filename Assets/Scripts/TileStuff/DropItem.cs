using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Tile))]
public class DropItem : MonoBehaviour
{
	public PlayerInventory.Item item = PlayerInventory.Item.Metals;

	public int MinDropAmount = 0;
	public int MaxDropAmount = 1;
	public float ChanceOfDrop = 1;

	void RemovedByPlayer()
	{
		if (Random.Range(0f, 1f) < ChanceOfDrop)
		{
			int amount = Random.Range(MinDropAmount, MaxDropAmount + 1);
			Log.instance.AddMessage(item + " +" + amount);
			PlayerInventory.inventory[item] += amount;
		}
	}
}
