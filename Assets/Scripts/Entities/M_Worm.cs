using UnityEngine;
using System.Collections;

public class M_Worm : M_Mob {
	
	protected override void levelAttributes() {
		health = getMod_health();
		mana = getMod_mana();
		manaPerSecond = getMod_manaPerSecond();
		attackPower = getMod_attackPower();
		defence = getMod_defence();
		critChance = getMod_critChance();
		grudgeCost = getMod_grudgeCost();
		// Talent: Regeneration
		if (TalentWindow.instance.isActive(id, 8)) {
			InvokeRepeating("regeneration", 1f, 1f);
		}
	}
	
	// Talent: Regeneration
	void regeneration() {
		health = Mathf.Clamp((int)(health + (maxHealth / 100f)), 0, maxHealth);
	}
	
	protected override int getMod_defence() {
		int mDefence = (int)(defence + mod_defence * GameController.instance.level);
		// Talent: Thick Armor
		if (TalentWindow.instance.isActive(id, 6)) {
			mDefence += 5;
		}
		// Talent: Thicker Armor
		if (TalentWindow.instance.isActive(id, 7)) {
			mDefence += 5;
		}
		return Mathf.Clamp(mDefence, 0, 80);
	}
	
	protected override int getMod_grudgeCost() {
		int mGrudge = grudgeCost;
		
		// Talent: Pullulate
		if (TalentWindow.instance.isActive(id, 5)) {
			mGrudge -= 1;
		}
		
		return mGrudge;
	}
	
	protected override int getMod_health() {
		int mHealth = (int)(health + mod_health * GameController.instance.level);
		// Talent: Pullulate
		if (TalentWindow.instance.isActive(id, 5)) {
			mHealth -= (int) ( mHealth * (30f / 100f));
		}
		return mHealth;
	}
	
	protected override void onDeath() {
		if (HUD != null)
			Destroy(HUD.gameObject);
		// Talent: Double Trouble
		if (TalentWindow.instance.isActive(id, 0)) {
			GameObject tw1 = Instantiate(GameController.instance.tinyWormPrefab, 
				transform.position + new Vector3(.1f, 0, 0), Quaternion.identity) as GameObject;
			GameObject tw2 = Instantiate(GameController.instance.tinyWormPrefab, 
				transform.position + new Vector3(-.1f, 0, 0), Quaternion.identity) as GameObject;
			float percent = 4f;
			// Talent: Double Double Trouble
			if (TalentWindow.instance.isActive(id, 2)) {
				percent = 2f;
			}
			tw1.GetComponent<M_TinyWorm>().health = (int) (maxHealth / percent);
			tw1.GetComponent<M_TinyWorm>().maxHealth = tw1.GetComponent<M_TinyWorm>().health;
			tw2.GetComponent<M_TinyWorm>().health = (int) (maxHealth / percent);
			tw2.GetComponent<M_TinyWorm>().maxHealth = tw2.GetComponent<M_TinyWorm>().health;
		}
	}
}
