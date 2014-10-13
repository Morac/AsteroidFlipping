using UnityEngine;
using System.Collections;

public class TileItemBtn : MonoBehaviour
{
	public Tile TileItem;
	public UILabel label;

	static GameObject previewObject;

	void Start()
	{
		label.text = TileItem.GetDisplayName();
	}

	void OnClick()
	{
		var tool = Player.Instance.MainTool;
		if(tool != null)
		{
			tool.SelectedTool = TileItem;
			
			if(previewObject != null)
			{
				Destroy(previewObject);
			}
			previewObject = (Instantiate(TileItem) as Tile).gameObject;
			previewObject.transform.position = tool.SelectionHightlight.transform.position;
			previewObject.transform.parent = tool.SelectionHightlight.transform;
			foreach(var collider in previewObject.GetComponentsInChildren<Collider>())
			{
				collider.enabled = false;
			}
			
		}
	}
}
