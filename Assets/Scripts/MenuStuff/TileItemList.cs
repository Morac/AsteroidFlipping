using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(UIGrid))]
public class TileItemList : MonoBehaviour
{

	public TileItemBtn prefab;

	public UILabel ErrorLabel;

	public RoomManager.Room CurrentRoom;

	void Update()
	{
		var tile = Player.Instance.CurrentTile;
		if(tile != null)
		{
			var room = tile.Room;
			if(room != CurrentRoom)
			{
				CurrentRoom = room;
				SetTiles();
			}
		}
	}

	void SetTiles()
	{
		foreach(var tile in GetComponentsInChildren<TileItemBtn>())
		{
			Destroy(tile.gameObject);
		}

		if(CurrentRoom == null || CurrentRoom.Type == RoomManager.RoomType.Unzoned)
		{
			ErrorLabel.text = "No Zone Set";
		}
		else
		{
			var tiles = TilePrefabList.Instance.GetAllTiles().Where(item => (item.RoomTypes | (int)CurrentRoom.Type) != 0).ToList();
			foreach(var tile in tiles)
			{
				var item = Instantiate(prefab, transform.position, transform.rotation) as TileItemBtn;
				item.transform.parent = transform;
				item.transform.localScale = Vector3.one;
				item.TileItem = tile;
			}

			Player.Instance.MainTool.SelectedTool = tiles[0];

			GetComponent<UIGrid>().Reposition();
		}
		//foreach(var tile in Tiles)
		//{
		//	var item = Instantiate(prefab, transform.position, transform.rotation) as TileItemBtn;
		//	item.transform.parent = transform;
		//	item.transform.localScale = Vector3.one;
		//	item.TileItem = tile;
		//}

		//Player.Instance.MainTool.SelectedTool = Tiles[0];

		//GetComponent<UIGrid>().Reposition();
	}

}
