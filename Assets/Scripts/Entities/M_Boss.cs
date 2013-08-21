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
	
	public override void showTooltip() {
		string tText = "[CF2945]" + eName + "[-]\n";
		tText += "[E6E477]'" + eDescription + "'[-]\n";
		tText += "Health: " + health + "\n";
		tText += "Mana: " + mana + "\n";
		tText += "Attack: " + attackPower + "\n";
		tText += "Defense: " + defence + "\n";
		tText += "Crit Chance: " + critChance + "%\n";
		C_Boss boss = GetComponent<C_Boss>();
		for (int i = 0 ; i < 5 ; i++) {
			if (boss.abilities[i] != null) {
				tText += "\n";
				tText += boss.abilities[i].tooltipMessageMock();
			}
		}
		UITooltip.ShowText(tText);
	}
}
