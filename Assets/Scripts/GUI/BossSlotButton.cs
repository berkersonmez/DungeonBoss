using UnityEngine;
using System.Collections;

public class BossSlotButton : MonoBehaviour {

	void OnClick() {
		BossSlot bS = transform.parent.GetComponent<BossSlot>();
		BossHolder.instance.boss = bS.prefab;
		SaveController.instance.startNewSave();
		Application.LoadLevel("s1");
	}
}
