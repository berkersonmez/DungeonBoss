using UnityEngine;
using System.Collections;

public class ShopSlotBG : MonoBehaviour {
	
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
		ShopSlot sS = transform.parent.GetComponent<ShopSlot>();
		sS.prefab.GetComponent<M_Mob>().showTooltip();
	}
	
	void onClick() {
		ShopSlot sS = transform.parent.GetComponent<ShopSlot>();
		if (sS.bought) {
			if (!sS.selected && Bottombar.instance.mobsInBar < 5) {
				Bottombar.instance.addToBar(sS.prefab);
				sS.toggleSelected();
			} else if (sS.selected) {
				Bottombar.instance.removeFromBar(sS.prefab);
				sS.toggleSelected();
			}
		}
	}
}
