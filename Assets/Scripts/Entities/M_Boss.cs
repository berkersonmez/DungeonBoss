using UnityEngine;
using System.Collections;

public class M_Boss : M_Entity {
	
	void Update () {
		update();
	}
	
	protected virtual void update() {
		if (health < 0) {
			onDeath();
			Destroy(gameObject);
			GameController.instance.gameOver();
		}
		hudHealth.sliderValue = (float) health / maxHealth;
	}
	
	protected virtual void onDeath() {
		if (HUD != null)
			Destroy(HUD.gameObject);
	}
}
