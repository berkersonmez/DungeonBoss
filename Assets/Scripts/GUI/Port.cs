using UnityEngine;
using System.Collections;

public class Port : MonoBehaviour {

	public int index;
	
	void OnPress(bool isDown) {
		if (isDown) {
			Invoke("onHoldDown", .5f);
		} else {
			if (IsInvoking("onHoldDown")) {
				CancelInvoke("onHoldDown");
				onClick();
			}
		}
	}
	
	void onHoldDown() {
		if (Bottombar.instance.barMobs[index] != null) {
			Bottombar.instance.barMobs[index].GetComponent<M_Entity>().showTooltip();
		}
	}
	
	void onClick() {
		Bottombar bB = transform.parent.parent.GetComponent<Bottombar>();
		bB.selectMob(index);
	}
}
