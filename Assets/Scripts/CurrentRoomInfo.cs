using UnityEngine;
using System.Collections;

public class CurrentRoomInfo : MonoBehaviour
{
	public UILabel RoomTypeLabel;
	public UILabel QualityLabel;

	public Color LowestQuality;
	public Color HighestQuality;

	RoomManager.Room CurrentRoom;
	RoomManager.RoomQuality CurrentQuality;

	void Update()
	{
		if (Player.Instance.CurrentTile != null)
		{
			if (CurrentRoom != Player.Instance.CurrentTile.Room)
			{
				CurrentRoom = Player.Instance.CurrentTile.Room;
				if (CurrentRoom != null)
					RoomTypeLabel.text = CurrentRoom.Type.ToString();
				else
					RoomTypeLabel.text = "";
			}
		}
		else
		{
			RoomTypeLabel.text = "";
		}

		if(CurrentRoom != null)
		{
			CurrentQuality = CurrentRoom.Quality;
			QualityLabel.text = QualityText(CurrentQuality);
			QualityLabel.color = QualityColour(CurrentQuality);
		}
		else
		{
			CurrentQuality = RoomManager.RoomQuality.None;
			QualityLabel.text = QualityText(RoomManager.RoomQuality.None);
			QualityLabel.color = Color.white;
		}
	}

	string QualityText(RoomManager.RoomQuality q)
	{
		return q + " quality";
	}

	Color QualityColour(RoomManager.RoomQuality q)
	{
		int i;
		var values = System.Enum.GetValues(typeof(RoomManager.RoomQuality));
		for(i = 0; i < values.Length; i++)
		{
			if ((int)values.GetValue(i) == (int)q)
				break;
		}

		float mod = (float)i / (float)(values.Length - 1);
		var r = (HighestQuality.r - LowestQuality.r) * mod + LowestQuality.r;
		var g = (HighestQuality.g - LowestQuality.g) * mod + LowestQuality.g;
		var b = (HighestQuality.b - LowestQuality.b) * mod + LowestQuality.b;

		return new Color(r, b, g);
	}
}
