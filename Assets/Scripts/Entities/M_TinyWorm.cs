using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class M_TinyWorm : M_Mob {
	
	protected override void update() {
		if (health < 0) {
			onDeath();
			Destroy(gameObject);
		}
		hudHealth.sliderValue = (float) health / maxHealth;
	}
	
	protected override void onDeath() {
		if (HUD != null)
			Destroy(HUD.gameObject);
		// Talent: Clinging Odor
		if (TalentWindow.instance.isActive(2, 1)) {
			List<M_Char> nearChars = findNearChars();
			foreach(M_Char nearChar in nearChars) {
				nearChar.getPoisoned(10f, 10);
			}
		}
	}
	
	private List<M_Char> findNearChars() {
		List<M_Char> nearTargets = new List<M_Char>();
		GameObject[] trgts = GameObject.FindGameObjectsWithTag("Char");
		foreach(GameObject trgt in trgts) {
			Vector3 diff = trgt.transform.position - transform.position;
            float distance = diff.sqrMagnitude;
            if (distance <= mainAttackRange) {
                nearTargets.Add(trgt.GetComponent<M_Char>());
            }
		}
		return nearTargets;
	}
}
