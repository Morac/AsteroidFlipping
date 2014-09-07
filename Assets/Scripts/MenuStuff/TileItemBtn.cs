using UnityEngine;
using System.Collections;

public class TileItemBtn : MonoBehaviour
{
	public Tile TileItem;
	public UILabel label;

	void Start()
	{
		label.text = TileItem.name;
	}

	void OnClick()
	{
		var tool = FindObjectOfType<PlayerTool>();
		if(tool != null)
		{
			tool.SelectedTool = TileItem;
		}
	}
}
