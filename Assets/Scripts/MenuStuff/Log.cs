using UnityEngine;
using System.Collections.Generic;

public class Log : MonoBehaviour {

	public static Log instance;

	void Awake()
	{
		instance = this;
	}

	const int logSize = 5;
	const float fadeTime = 3;
	public UILabel LogItemPrefab;

	Queue<UILabel> queue = new Queue<UILabel>();

	public void AddMessage(string msg)
	{
		while(queue.Count >= logSize)
		{
			var l = queue.Dequeue();
			Destroy(l.gameObject);
		}

		foreach(var item in queue)
		{
			item.transform.localPosition += Vector3.up * item.height;
		}

		var label = Instantiate(LogItemPrefab, transform.position, transform.rotation) as UILabel;
		label.transform.parent = transform;
		label.transform.localScale = Vector3.one;
		label.text = msg;
		

		queue.Enqueue(label);
	}
}
