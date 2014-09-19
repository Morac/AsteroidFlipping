using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UILabel))]
public class DisplayTool : MonoBehaviour
{
	UILabel label;
	PlayerTool tool;

	void Start()
	{
		label = GetComponent<UILabel>();
		tool = FindObjectOfType<PlayerTool>();
	}

	void Update()
	{
		label.text = tool.SelectedTool.GetDisplayName();
	}
}
