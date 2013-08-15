using UnityEngine;
using System.Collections;

public class M_Skeleton : M_Mob {
	
	protected int baseAttackPower;
	protected float nearbyRange = 5f;
	
	protected override int getMod_critChance() {
		int mCrit = (int)(critChance + mod_critChance * getLevel());
		// Talent: Spiny Sword
		if (TalentWindow.instance.isActive(id, 3)) {
			mCrit += (int) (mCrit / 10f);
		}
		return Mathf.Clamp(mCrit, 0, 80);
	}
	
	protected override float getMod_mainAttackRange() {
		float mMAR = mainAttackRange;
		// Talent: Slendy
		if (TalentWindow.instance.isActive(id, 4)) {
			mMAR += (mMAR / 2f);
		}
		return mMAR;
	}
	
	protected override int getMod_attackPower() {
		int mAP = (int)(attackPower + mod_attackPower * getLevel());
		// Talent: Empowered
		if (TalentWindow.instance.isActive(id, 1)) {
			mAP += (int) (mAP * (5f / 100f));
		}
		return mAP;
	}
	
	protected override int getLevel() {
		// Talent: Fast Learner
		if (TalentWindow.instance.isActive(id, 5)) {
			return GameController.instance.level + 5;
		}
		return GameController.instance.level;
	}
	
	protected override int getMod_grudgeCost() {
		int mGrudge = grudgeCost;
		
		// Talent: Pacified
		if (TalentWindow.instance.isActive(id, 2)) {
			mGrudge -= 2;
		}
		
		return mGrudge;
	}
	
	protected int countNearbySkeletons() {
		int count = 0;
		GameObject[] trgts = GameObject.FindGameObjectsWithTag("Mob");
		foreach(GameObject trgt in trgts) {
			if (trgt.GetComponent<M_Skeleton>() != null) {
				Vector3 diff = trgt.transform.position - transform.position;
	            float distance = diff.sqrMagnitude;
	            if (distance <= nearbyRange) {
					count++;
	            }
			}
		}
		return count-1;
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
		
		baseAttackPower = attackPower;
		// Talent: United
		if (TalentWindow.instance.isActive(id, 0)) {
			InvokeRepeating("buffAPByCountingSkeletons", .5f, .5f);
		}
		
		// Talent: Merry-Go-Round
		if (TalentWindow.instance.isActive(id, 6)) {
			GetComponent<S_CircularSlash>().enabled = true;
		}
		
		// Talent: Hard Hitter
		if (TalentWindow.instance.isActive(id, 7)) {
			int mSP = GetComponent<S_CircularSlash>().spellPower;
			mSP += (int) (mSP / 10f); 
			GetComponent<S_CircularSlash>().spellPower = mSP;
		}
		
		// Talent: Ferris Wheel
		if (TalentWindow.instance.isActive(id, 8)) {
			float mRange = GetComponent<S_CircularSlash>().range;
			mRange += (mRange / 2f); 
			GetComponent<S_CircularSlash>().range = mRange;
		}
	}
	
	void buffAPByCountingSkeletons() {
		attackPower = (int) ( baseAttackPower + ( countNearbySkeletons() * baseAttackPower * (5f / 100f) ));
		Debug.Log("AP: " + attackPower);
	}
}
