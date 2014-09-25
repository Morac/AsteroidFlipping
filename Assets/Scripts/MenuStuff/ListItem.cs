using UnityEngine;
using System.Collections;

public class ListItem : MonoBehaviour {

	public delegate void ButtonCallback(ListItem source);

	public object Data;

	public UILabel Label0;
	public UILabel Label1;
	public UILabel Label2;

	public ButtonCallback Button0Clicked;
	public ButtonCallback Button1Clicked;
	public ButtonCallback Button2Clicked;

	public void Click0()
	{
		if(Button0Clicked != null)
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
