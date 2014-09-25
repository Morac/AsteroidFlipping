using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Tile))]
public class TileEditor : Editor
{
	public override void OnInspectorGUI()
	{
		var tile = target as Tile;

		foreach(var t in TilePrefabList.Instance.GetAllTiles())
		{
			if (t.name == tile.name)
				continue;
			if(t.SaveCode == tile.SaveCode)
			{
				Color c = GUI.color;
				GUI.color = Color.red;
				GUILayout.Box("Save Code is in use by another tile");
				GUI.color = c;
				break;
			}
		}

		base.OnInspectorGUI();
	}
}
