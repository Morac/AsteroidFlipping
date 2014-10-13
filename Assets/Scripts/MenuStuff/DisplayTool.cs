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
		tool = Player.Instance.MainTool;
	}

	void Update()
	{
		label.text = tool.SelectedTool.GetDisplayName();
	}
}
