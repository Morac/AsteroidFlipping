using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UILabel))]
public class DisplayToolCost : MonoBehaviour
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
		label.text = tool.SelectedTool.CostString();
	}
}
