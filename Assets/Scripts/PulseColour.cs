using UnityEngine;
using System.Collections;

public class PulseColour : MonoBehaviour
{
	public float Period = 1f;

	Color baseColour;

	void Start()
	{
		baseColour = renderer.material.color;
	}

	void Update()
	{
		float f = Mathf.Sin(Time.time * 2 * Mathf.PI / Period) / 4f + 0.75f;
		Color c = baseColour * f;
		c.a = 1;
		renderer.material.color = c;
	}
}
