using UnityEngine;
using System.Collections;

public class C_BossDoor : MonoBehaviour {
	
	public Vector3 bossroomPos;
	
	int grudgeCost = 30;
	
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Char") {
			other.transform.position = bossroomPos;
			InputController.instance.moveTo(other.transform.position);
			GameController.instance.gameOver();
		}
    }
}