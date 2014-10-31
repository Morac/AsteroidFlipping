using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Tile))]
[CanEditMultipleObjects]
public class TileEditor : Editor
{
	SerializedProperty roomTypes;

	void OnEnable()
	{
		roomTypes = serializedObject.FindProperty("RoomTypes");
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		var tile = target as Tile;
		if (tile == null)
			return;

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

		roomTypes.intValue = EditorGUILayout.MaskField("Room Types", roomTypes.intValue, System.Enum.GetNames(typeof(RoomManager.RoomType)));

		serializedObject.ApplyModifiedProperties();
	}
}
