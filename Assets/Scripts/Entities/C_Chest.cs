using UnityEngine;
using System.Collections;

public class C_Chest : MonoBehaviour {

	int grudgeCost = 30;
	
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Char") {
			GameController.instance.gainGrudge(grudgeCost / 2);
			Destroy(this.gameObject);
		}
    }
}
