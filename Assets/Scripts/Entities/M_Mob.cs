using UnityEngine;
using System.Collections;

public class M_Mob : M_Entity {
	
	Talent[] talents;
	
	public int goldCost = 1000;
	public int grudgeCost = 100;
	
	protected virtual int getMod_grudgeCost() {
		int mGrudge = grudgeCost;
		return mGrudge;
	}
	
	protected override void levelAttributes() {
		health = getMod_health();
		mana = getMod_mana();
		manaPerSecond = getMod_manaPerSecond();
		attackPower = getMod_attackPower();
		defence = getMod_defence();
		critChance = getMod_critChance();
		grudgeCost = getMod_grudgeCost();
		mainAttackRange = getMod_mainAttackRange();
	}
	
	void Update () {
		update();
	}
	
	protected virtual void update() {
		if (health < 0) {
			GameController.instance.despawnUnit();
			GameController.instance.gainGrudge(grudgeCost/2);
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
		string tText = "[CF2945]" + eName + "[-]\n";
		tText += "[E6E477]'" + eDescription + "'[-]\n";
		tText += "Health: " + getMod_health() + "\n";
		tText += "Mana: " + getMod_mana() + "\n";
		tText += "Attack: " + getMod_attackPower() + "\n";
		tText += "Defense: " + getMod_defence() + "\n";
		tText += "Crit Chance: " + getMod_critChance() + "%\n";
		tText += "\n";
		if (GameController.instance.grudge >= grudgeCost)
			tText += "[77E677]";
		else
			tText += "[ED1818]";
		tText += "$grudge " + getMod_grudgeCost() + "[-]";
		UITooltip.ShowText(tText);
	}
	
	public void spawn(Vector3 position) {
		if (GameController.instance.grudge >= getMod_grudgeCost()) {
			Bottombar.instance.spawnMob(position);
			GameController.instance.grudge -= getMod_grudgeCost();
		}
	}
}
