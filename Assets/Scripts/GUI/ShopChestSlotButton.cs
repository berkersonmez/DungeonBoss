using UnityEngine;
using System.Collections;

public class ShopChestSlotButton : MonoBehaviour {
	
	public GameObject mobWindow;

	void OnClick() {
		ShopChestSlot cS = transform.parent.GetComponent<ShopChestSlot>();
		if (cS.locked) {
			if (GameController.instance.level >= cS.unlocksAt) {
				cS.unlock();
			}
		} else if (GameController.instance.gameState == (int) GameController.GameState.PREPARATION && GameController.instance.gold >= cS.prefab.GetComponent<C_Chest>().goldCost) {
			NGUITools.SetActive(mobWindow, false);
			Bottombar.instance.startChestSpawnMode(cS.prefab);
		}
	}
}
