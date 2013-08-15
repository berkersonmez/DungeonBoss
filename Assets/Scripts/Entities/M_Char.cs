using UnityEngine;
using System.Collections;

public class M_Char : M_Entity {
	
	public int experience = 10;
	public int carriedGold = 20;
	
	void Update () {
		update();
	}
	
	protected virtual void update() {
		if (health < 0) {
			GameController.instance.gainExp(experience);
			GameController.instance.gainGold(carriedGold);
			onDeath();
			Destroy(gameObject);
		}
		hudHealth.sliderValue = (float) health / maxHealth;
	}
	
	protected virtual void onDeath() {
		if (HUD != null)
			Destroy(HUD.gameObject);
	}
	
	public override void showTooltip() {
		string tText = "[CFCC29]" + eName + "[-]\n";
		tText += "Attack: " + attackPower + "\n";
		tText += "Defense: " + defence + "\n";
		tText += "Crit Chance: " + critChance + "%\n";
		tText += "[CFCFCF]'" + eDescription + "'[-]\n";
		tText += "\n";
		tText += "Carries: $gold " + carriedGold + " $exp " + experience;
		
		foreach(Spell spell in GetComponents<Spell>()) {
			if (spell.enabled) {
				tText += "\n\n";
				tText += spell.tooltipMessage();
			}
		}
		
		UITooltip.ShowText(tText);
	}
}
