using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UIProgressBar))]
public class BuildProgressBar : MonoBehaviour
{

	public GameObject SelectionObject;
	public PlayerTool Tool;

	Camera thisCamera;
	UIProgressBar bar;

	void Start()
	{
		bar = GetComponent<UIProgressBar>();
		thisCamera = GetComponentInParent<Camera>();
	}


	void Update()
	{
		var view = Camera.main.WorldToViewportPoint(SelectionObject.transform.position + Vector3.up * SelectionObject.transform.localScale.y / 2);
		transform.localPosition = thisCamera.ViewportToScreenPoint(view) -
			new Vector3(
				Screen.width / 2,
				Screen.height / 2,
				0);
	}

	void OnGUI()
	{
		if (Tool.Percent == 0)
		{
			bar.foregroundWidget.gameObject.SetActive(false);
			bar.backgroundWidget.gameObject.SetActive(false);
		}
		else
		{
			bar.foregroundWidget.gameObject.SetActive(true);
			bar.backgroundWidget.gameObject.SetActive(true);
			bar.value = Tool.Percent;
		}
	}
}
