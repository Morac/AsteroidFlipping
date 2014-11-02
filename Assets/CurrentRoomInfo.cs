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

		if(CurrentRoom != null) //&& quality is different
		{
			
		}
		else if(CurrentQuality != RoomManager.RoomQuality.None)
		{

		}
	}
}
