using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UIGrid))]
public class TileItemList : MonoBehaviour
{

	public Tile[] Tiles { get { return PlayerInventory.GetUsableTiles(); } }
	public TileItemBtn prefab;

	void Start()
	{
		foreach(var tile in Tiles)
		{
			var item = Instantiate(prefab, transform.position, transform.rotation) as TileItemBtn;
			item.transform.parent = transform;
			item.transform.localScale = Vector3.one;
			item.TileItem = tile;
		}

		Player.Instance.MainTool.SelectedTool = Tiles[0];

		GetComponent<UIGrid>().Reposition();
	}

}
