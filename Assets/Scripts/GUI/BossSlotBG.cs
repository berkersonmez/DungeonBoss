using UnityEngine;
using System.Collections;

public class BossSlotBG : MonoBehaviour {

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
		BossSlot bS = transform.parent.GetComponent<BossSlot>();
		bS.prefab.GetComponent<M_Boss>().showTooltip();
	}
	
	void onClick() {
	}
}
