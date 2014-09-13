using UnityEngine;
using System.Collections;

public class BlueprintList : MonoBehaviour
{

	public BlueprintCard prefab;

	void Start()
	{
		foreach(var tile in TilePrefabList.Instance.PurchasableTiles)
		{
			if(!PlayerInventory.IsUnlocked(tile))
			{
				var inst = Instantiate(prefab, transform.position, transform.rotation) as BlueprintCard;
				inst.transform.parent = transform;
				inst.transform.localScale = Vector3.one;
				inst.Init(tile);
			}
		}
	}

}
