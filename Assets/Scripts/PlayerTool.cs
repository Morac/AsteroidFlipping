using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PlayerTool : MonoBehaviour
{

	Camera cam;

	public float Percent
	{
		get
		{
			if (endTime == 0)
				return 0;
			return Mathf.Max(0, 1 - (endTime - Time.time) / buildTime);
		}
	}

	float endTime;
	const float buildTime = 1;


	Tile selected = null;

	public GameObject SelectionHightlight;
	public Tile SelectedTool;

	void Start()
	{
		cam = Camera.main;
	}

	void Update()
	{
		Tile tile = GetSelectedTile();
		if (tile != null)
		{
			SelectionHightlight.transform.position = tile.transform.position;
			if (selected != tile)
			{
				endTime = 0;
				selected = tile;
			}

			if (IsValid(tile))
			{
				SelectionHightlight.renderer.material.color = transparent(Color.green);
			}
			else
			{
				SelectionHightlight.renderer.material.color = transparent(Color.red);
			}

			if (Input.GetButton("Fire1") && IsValid(tile))
			{
				if (endTime == 0)
				{
					endTime = Time.time + buildTime;
				}
				if (endTime < Time.time)
				{
					tile.SetTile(SelectedTool);
				}
			}
			else
			{
				endTime = 0;
			}
		}


	}

	Color transparent(Color c)
	{
		Color r = c;
		r.a = 0.5f;
		return r;
	}

	public bool IsValid(Tile t)
	{
		return t.name != SelectedTool.name && t.type == Tile.TileType.Interior;
	}

	Tile GetSelectedTile()
	{
		List<Tile> possible = new List<Tile>();
		foreach (var hit in Physics.RaycastAll(transform.position, -transform.up))
		{
			var tile = hit.transform.GetComponentInParents<Tile>();
			if (tile == null)
			{
				continue;
			}
			possible.AddRange(tile.AdjacentTiles(true));
			possible.Add(tile);
			break;
		}

		if (possible.Count > 0)
		{
			var closest = possible.First(item => item != null);
			if (closest == null)
				return null;

			var p1 = cam.WorldToViewportPoint(closest.transform.position);
			var p2 = cam.ScreenToViewportPoint(Input.mousePosition);
			float min = (p1 - p2).sqrMagnitude;
			foreach (var tile in possible)
			{
				if (tile == null)
					continue;
				p1 = cam.WorldToViewportPoint(tile.transform.position);
				float dist = (p1 - p2).sqrMagnitude;
				if (dist < min)
				{
					closest = tile;
					min = dist;
				}
			}
			return closest;
		}

		return null;
	}
}
