using UnityEngine;
using System.Collections;

public class SaveSlotButton : MonoBehaviour {

	void OnClick() {
		SaveSlot sS = transform.parent.GetComponent<SaveSlot>();
		sS.selectThisSlot();
		Application.LoadLevel("s1");
	}
}
