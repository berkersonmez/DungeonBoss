using UnityEngine;
using System.Collections;

public class C_Mage : C_Char {
	protected override void initialize() {
		
		InvokeRepeating("targetNearMob", 1f, 2f);
		sm = GameObject.Find("_SpriteManager").GetComponent<LinkedSpriteManager>();
		
		sprite = sm.AddSprite(this.gameObject, 1.5f, 1.5f, sm.PixelCoordToUVCoord(7*96, 10*96), sm.PixelSpaceToUVSpace(96, 96), Vector3.zero, false);
		walkRight = new UVAnimation();
		walkRight.BuildUVAnim(sm.PixelCoordToUVCoord(0, 10*96), sm.PixelSpaceToUVSpace(96, 96), 8, 1, 8, 10f);
		walkRight.loopCycles = -1;
		sprite.AddAnimation(walkRight);
		
		walkLeft = new UVAnimation();
		walkLeft.BuildUVAnim(sm.PixelCoordToUVCoord(0*96, 11*96), sm.PixelSpaceToUVSpace(96, 96), 8, 1, 8, 10f);
		walkLeft.loopCycles = -1;
		sprite.AddAnimation(walkLeft);
		
		standRight = new UVAnimation();
		standRight.BuildUVAnim(sm.PixelCoordToUVCoord(7*96, 10*96), sm.PixelSpaceToUVSpace(96, 96), 2, 1, 2, 1f);
		standRight.loopCycles = -1;
		sprite.AddAnimation(standRight);
		
		standLeft = new UVAnimation();
		standLeft.BuildUVAnim(sm.PixelCoordToUVCoord(7*96, 11*96), sm.PixelSpaceToUVSpace(96, 96), 2, 1, 2, 1f);
		standLeft.loopCycles = -1;
		sprite.AddAnimation(standLeft);
		
		attackRight = new UVAnimation();
		attackRight.BuildUVAnim(sm.PixelCoordToUVCoord(8*96, 10*96), sm.PixelSpaceToUVSpace(96, 96), 2, 1, 2, 4f);
		attackRight.loopCycles = 0;
		sprite.AddAnimation(attackRight);
		
		attackLeft = new UVAnimation();
		attackLeft.BuildUVAnim(sm.PixelCoordToUVCoord(8*96, 11*96), sm.PixelSpaceToUVSpace(96, 96), 2, 1, 2, 4f);
		attackLeft.loopCycles = 0;
		sprite.AddAnimation(attackLeft);
		
		state = (int) State.SEEKING;
		
	}
	
	override protected void attackMobIfNear() {
		if (Vector3.Distance(transform.position, targetMob.position) <= character.mainAttackRange) {
			state = (int) State.ATTACKING;
			charAI.canMove = false;
			Invoke("endMeleeCooldown", character.mainAttackCooldown);
			
			animateAttack(targetMob.GetComponent<M_Mob>());
			mainAttackAnim(targetMob.GetComponent<M_Mob>());
		}
	}
	
	override protected void mainAttackAnim(M_Entity other) {
		Vector3 posDiff = other.transform.position - transform.position;
		posDiff.y = 0;
		GameObject attackEffect = Instantiate(mainAttackEffect, transform.position, transform.rotation) as GameObject;
		attackEffect.GetComponent<E_Mageball>().initialize(posDiff.normalized, character);
	}
}
