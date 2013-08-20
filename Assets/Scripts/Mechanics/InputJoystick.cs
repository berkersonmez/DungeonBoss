using UnityEngine;
using System.Collections;

public class InputJoystick : MonoBehaviour {
	
	private Vector3 dragOrigin;
	public bool dragging;
	public float dragSpeed = 4;
	
	void OnPress(bool isDown) {
		if (isDown) {
			dragging = true;
			dragOrigin = Input.mousePosition;
		} else {
			
		}
	}
	
	void Update() {
		if (dragging) {
			if (Input.GetButtonUp("Fire1")) {
				dragging = false;
				return;
			}
			
			Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
			if (pos.magnitude > .15f) return;
			if (GameController.instance.gameState == (int) GameController.GameState.BOSSFIGHT) {
				Vector3 moveBoss = new Vector3(pos.x, 0, pos.y);
        		GameController.instance.bossC.move(moveBoss);
			} else {
				Vector3 move = new Vector3(pos.x * dragSpeed, 0, pos.y * dragSpeed);
        		Camera.main.transform.Translate(move, Space.World);
			}
			
        	
        }
	}
	
}
