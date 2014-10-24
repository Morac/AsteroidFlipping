using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PlayerTool : MonoBehaviour
{
	public string InputString = "Fire1";
	public string CancelString = "Fire2";
	public bool InvertedProgressBar = false;

	Camera cam;

	public float Percent
	{
		get
		{
			if (endTime == 0)
				return 0;
			var i = Mathf.Max(0, 1 - (endTime - Time.time) / buildTime);
			if (InvertedProgressBar)
				return 1-i;
			else
				return i;
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
			if (SelectionHightlight != null)
				SelectionHightlight.transform.position = tile.transform.position;
			if (selected != tile)
			{
				endTime = 0;
				selected = tile;
			}

			if (IsValid(tile))
			{
				if (SelectionHightlight != null)
					SelectionHightlight.renderer.material.color = transparent(Color.green);
			}
			else
			{
				if (SelectionHightlight != null)
					SelectionHightlight.renderer.material.color = transparent(Color.red);
			}

			if (Input.GetButton(InputString) && !Input.GetButton(CancelString) && IsValid(tile) && !GlobalSettings.HitUI())
			{
				if (endTime == 0)
				{
					endTime = Time.time + buildTime;
				}
				if (endTime < Time.time)
				{
					tile.BroadcastMessage("RemovedByPlayer", SendMessageOptions.DontRequireReceiver);
					var newtile = tile.SetTile(SelectedTool);
					if (newtile != null)
					{
						newtile.BroadcastMessage("PlacedByPlayer", SendMessageOptions.DontRequireReceiver);
					}
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
		return t.name != SelectedTool.name 
			&& t.type == Tile.TileType.Interior 
			&& SelectedTool.CanAfford() 
			&& RoomManager.Instance.VerifyReplace(t, SelectedTool);
	}

	Tile GetSelectedTile()
	{
		List<Tile> possible = new List<Tile>();
		if (Player.Instance.CurrentTile != null)
		{
			possible.AddRange(Player.Instance.CurrentTile.AdjacentTiles(true));
			if (SelectedTool.CanPlaceOnTopOfCharacter)
				possible.Add(Player.Instance.CurrentTile);
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
