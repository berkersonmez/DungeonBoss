using UnityEngine;
using System.Collections;

public class ShopSlotButton : MonoBehaviour {

	void OnClick() {
		ShopSlot sS = transform.parent.GetComponent<ShopSlot>();
		if (sS.locked) {
			if (GameController.instance.level >= sS.unlocksAt) {
				sS.unlock();
			}
		} else if (!sS.bought && GameController.instance.gold >= sS.prefab.GetComponent<M_Mob>().goldCost) {
			GameController.instance.gold -= sS.prefab.GetComponent<M_Mob>().goldCost;
			sS.changeButtonToTalent();
		} else {
			TalentWindow.instance.openWindow(sS.prefab.GetComponent<M_Mob>().id);
			NGUITools.SetActive(GameController.instance.talentWindow, true);
		}
	}
}
