using UnityEngine;
using System.Collections;

public static class GOExtensions
{
	public static T GetComponentInParents<T>(this GameObject go) where T : Behaviour
	{
		var current = go;
		while(current != null)
		{
			var comp = current.GetComponent<T>();
			if (comp != null)
				return comp;
			else
				current = current.transform.parent != null ? current.transform.parent.gameObject : null;
		}
		return null;

	}
}

public static class MonoExtensions
{
	public static T GetComponentInParents<T>(this MonoBehaviour mb) where T : Behaviour
	{
		return mb.gameObject.GetComponentInParents<T>();
	}
}

public static class TransformExtensions
{
	public static T GetComponentInParents<T>(this Transform t) where T : Behaviour
	{
		return t.gameObject.GetComponentInParents<T>();
	}
}

public static class CameraExtensions
{
	public static void Shake(this Camera c, float amount, float duration)
	{
		iTween.ShakePosition(c.gameObject, Vector3.one * amount, duration);
	}
}