using UnityEngine;
using System.Collections;

public class ShopChestSlotBG : MonoBehaviour {

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
		ShopChestSlot cS = transform.parent.GetComponent<ShopChestSlot>();
		cS.prefab.GetComponent<C_Chest>().showTooltip();
	}
	
	void onClick() {
	}
}
