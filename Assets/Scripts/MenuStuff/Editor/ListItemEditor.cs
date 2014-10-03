using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

[CustomEditor(typeof(ListItem))]
public class ListItemEditor : Editor
{

	bool labelfoldout = false;
	bool groupfoldout = false;

	public override void OnInspectorGUI()
	{
		var item = target as ListItem;

		var list = item.GetComponentInParents<GenericUIList>();


		GUILayout.Label("--Labels--");
		int n = 0;
		if (list && list.LabelNames() != null)
		{
			for (int i = 0; i < list.LabelNames().Count; i++, n++)
			{
				if (item.Labels.Count <= i)
					item.Labels.Add(null);
				item.Labels[i] = EditorGUILayout.ObjectField(list.LabelNames()[i], item.Labels[i], typeof(UILabel), true) as UILabel;
			}
		}

		EditorGUI.indentLevel++;
		labelfoldout = EditorGUILayout.Foldout(labelfoldout, "Other labels");
		if (labelfoldout)
		{
			EditorGUI.indentLevel++;
			int therest = item.Labels.Count - n;
			int newsize = EditorGUILayout.IntField(therest);
			if (newsize < 0)
				newsize = 0;
			for (int i = 0; i < therest; i++)
			{
				if (item.Labels.Count <= i)
					item.Labels.Add(null);
				item.Labels[i + n] = EditorGUILayout.ObjectField(item.Labels[i + n], typeof(UILabel), true) as UILabel;
			}

			if (newsize > therest)
			{
				for (; therest < newsize; therest++)
				{
					item.Labels.Add(null);
				}
			}
			else if (newsize < therest)
			{
				for (; newsize < therest; newsize++)
				{
					item.Labels.RemoveAt(item.Labels.Count - 1);
				}
			}
			EditorGUI.indentLevel--;
		}
		EditorGUI.indentLevel--;


		EditorGUILayout.Space();
		EditorGUILayout.Space();

		GUILayout.Label("--Groups--");

		n = 0;
		if(list && list.GroupNames() != null)
		{
			for (int i = 0; i < list.GroupNames().Count; i++, n++)
			{
				if (item.GameObjects.Count <= i)
					item.GameObjects.Add(null);
				item.GameObjects[i] = EditorGUILayout.ObjectField(list.GroupNames()[i], item.GameObjects[i], typeof(GameObject), true) as GameObject;
			}
		}

		EditorGUI.indentLevel++;
		groupfoldout = EditorGUILayout.Foldout(groupfoldout, "Other groups");
		if(groupfoldout)
		{
			EditorGUI.indentLevel++;

			int thereset = item.GameObjects.Count - n;
			int newsize = EditorGUILayout.IntField(thereset);
			if (newsize < 0)
				newsize = 0;
			for (int i = 0; i < thereset; i++)
			{
				if (item.GameObjects.Count <= i)
					item.GameObjects.Add(null);
				item.GameObjects[i + n] = EditorGUILayout.ObjectField(item.GameObjects[i + n], typeof(GameObject), true) as GameObject;
			}

			if(newsize > thereset)
			{
				for(; thereset < newsize; thereset++)
				{
					item.GameObjects.Add(null);
				}
			}
			else if(newsize < thereset)
			{
				for(; newsize < thereset; newsize++)
				{
					item.GameObjects.RemoveAt(item.GameObjects.Count - 1);
				}
			}

			EditorGUI.indentLevel--;
		}
		EditorGUI.indentLevel--;

		if (GUI.changed)
		{
			EditorUtility.SetDirty(target);
		}
	}

}
