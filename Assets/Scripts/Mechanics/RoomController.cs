using UnityEngine;
using System.Collections;

public class RoomController : MonoBehaviour {

	public bool locked = true;
	public int cost;
	public GameObject[] chests = new GameObject[2];
	public RoomUnlockButton unlockButton;
	public UILabel l_unlockButton;
	
	
	void Start () {
		if (!locked) {
			renderer.enabled = false;
			Destroy(transform.Find("Unlock Button").gameObject);
		}
		unlockButton = transform.Find("Unlock Button").Find("Button").GetComponent<RoomUnlockButton>();
		l_unlockButton = transform.Find("Unlock Button").Find("Label").GetComponent<UILabel>();
		l_unlockButton.text = "Unlock Room $gold" + cost;
		unlockButton.rc = this;
	}
	
	public void unlock() {
		locked = false;
		GameController.instance.gold -= cost;
		renderer.enabled = false;
		Destroy(transform.Find("Unlock Button").gameObject);
	}
	
	public bool isFull() {
		return (chests[0] != null && chests[1] != null);
	}
	
	public void addChest(GameObject chest) {
		if (chests[0] == null) {
			chests[0] = chest;
		} else if (chests[1] == null) {
			chests[1] = chest;
		}
	}
	
}
