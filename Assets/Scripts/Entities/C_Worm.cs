using UnityEngine;
using System.Collections;

public class C_Worm : C_Mob {
	protected override void attackCharIfNear() {
		if (Vector3.Distance(transform.position, targetChar.position) <= monster.mainAttackRange) {
			state = (int) State.ATTACKING;
			mobAI.canMove = false;
			Invoke("endMeleeCooldown", monster.mainAttackCooldown);
			
			// Talent: Clinging Odor
			// Talent: Halitosis
			if (TalentWindow.instance.isActive(monster.id, 4)) {
				if (Random.Range(0, 10) == 0) {
					targetChar.GetComponent<M_Char>().getPoisoned(10f, 10);
				}
			} else if (TalentWindow.instance.isActive(monster.id, 3)) {
				if (Random.Range(0, 10) == 0) {
					targetChar.GetComponent<M_Char>().getPoisoned(5f, 10);
				}
			}
			
			animateAttack(targetChar.GetComponent<M_Char>());
			monster.damage(targetChar.GetComponent<M_Char>());
		}
	}
}
