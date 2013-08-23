using UnityEngine;
using System.Collections;

public class RoomUnlockButton : MonoBehaviour {
	
	public RoomController rc;

	public void onClick() {
		if (GameController.instance.gold >= rc.cost) {
			rc.unlock();
		}
	}
}
