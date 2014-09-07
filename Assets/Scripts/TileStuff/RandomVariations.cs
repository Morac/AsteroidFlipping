using UnityEngine;
using System.Collections;

public class RandomVariations : MonoBehaviour
{

	public float ScaleVariation = 0.1f;
	
	void Start()
	{
		transform.localRotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));
		transform.localScale *= Random.Range(1 - ScaleVariation, 1 + ScaleVariation);
	}

}
