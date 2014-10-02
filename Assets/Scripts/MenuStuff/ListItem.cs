using UnityEngine;
using System.Collections.Generic;

public class ListItem : MonoBehaviour
{

	public delegate void ButtonCallback(ListItem source);

	public object Data;

	public List<UILabel> Labels = new List<UILabel>();

	public List<GameObject> GameObjects = new List<GameObject>();

	public ButtonCallback Button0Clicked;
	public ButtonCallback Button1Clicked;
	public ButtonCallback Button2Clicked;

	public void Click0()
	{
		if (Button0Clicked != null)
			Button0Clicked(this);
	}

	public void Click1()
	{
		if (Button1Clicked != null)
			Button1Clicked(this);
	}

	public void Click2()
	{
		if (Button2Clicked != null)
			Button2Clicked(this);
	}
}
