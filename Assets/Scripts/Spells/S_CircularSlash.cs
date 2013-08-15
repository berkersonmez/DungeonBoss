using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S_CircularSlash : Spell {
	
	public string targetMode = "Mob";
	public float range;
	public int minTargetsToCast = 2;
	public GameObject effect;
	
	private List<M_Entity> nearTargets;

	override public void initialize() {
		sName = "Circular Slash";
		description = "Inflicts damage with " + spellPower + " spell power to all " +
			"targets within " + range + " range.";
		nearTargets = new List<M_Entity>();
		InvokeRepeating("checkCondition", 1f, .5f);
	}
	
	void checkCondition() {
		if (IsInvoking("spellCooldown")) return;
		if (entityM.mana >= manaCost) {
			if (countNearTargets() >= minTargetsToCast) {
				cast();
			}
		}
	}
	
	void cast() {
		animate();
		entityM.mana -= manaCost;
		foreach(M_Entity target in nearTargets) {
			entityM.spellDamage(target, spellPower);
		}
		Invoke("spellCooldown", cooldown);
	}
	
	void animate() {
		Instantiate(effect, transform.position, transform.rotation);
	}
	
	int countNearTargets() {
		int count = 0;
		nearTargets.Clear();
		GameObject[] trgts = GameObject.FindGameObjectsWithTag(targetMode);
		Transform foundTrgt = null;
		foreach(GameObject trgt in trgts) {
			Vector3 diff = trgt.transform.position - transform.position;
            float distance = diff.sqrMagnitude;
            if (distance <= range) {
				count++;
                nearTargets.Add(trgt.GetComponent<M_Entity>());
            }
		}
		return count;
	}
}
