using UnityEngine;
using System.Collections;

public class S_ConeSlash : Spell {

	public string targetMode = "Char";
	public float range;
	public GameObject effect;
	
	override public void initialize() {
		sName = "Cone Slash";
		description = "Inflicts damage with " + entityM.attackPower + " attack power to all " +
			"targets in a cone.";
		spriteName = "t_skeleton_8";
		if (gameObject.tag != "Boss") {
			InvokeRepeating("checkCondition", 1f, .5f);
		}
	}
	
	override public void checkCondition() {
		if (IsInvoking("spellCooldown")) return;
		if (entityM.mana >= manaCost) {
			cast();
		}
	}
	
	void cast() {
		animate();
		entityM.mana -= manaCost;
		GameObject[] chars = GameObject.FindGameObjectsWithTag("Char");
		foreach(GameObject charObj in chars) {
			Vector3 distVector = charObj.transform.position - transform.position;
			if (distVector.magnitude <= range) {
				
				if (Vector3.Angle(crosshair.position - transform.position, distVector) < 45) {
					entityM.damage(charObj.GetComponent<M_Entity>());
				}
			}
		}
		Invoke("spellCooldown", cooldown);
	}
	
	void animate() {
		Vector3 posDiff = crosshair.position - transform.position;
		float angle = Mathf.Atan2(posDiff.z, -posDiff.x) * Mathf.Rad2Deg;
		Quaternion animRot = Quaternion.Euler(new Vector3(0, 180+angle, 0));
		Instantiate(effect, crosshair.position, animRot);
	}
}
