using UnityEngine;
using System.Collections;

public class C_Char : C_Entity {
	
	public float mobAttentionRadius = 2f;
	public GameObject mainAttackEffect;
	
	protected EntityAI charAI;
	protected M_Char character;
	protected Transform targetChest;
	protected Transform targetMob;
	protected Transform targetBoss;

	void Start () {
		charAI = GetComponent<EntityAI>();
		character = GetComponent<M_Char>();
		initialize();
	}
	
	protected virtual void initialize() {
		InvokeRepeating("targetNearMob", 1f, 2f);
		sm = GameObject.Find("_SpriteManager").GetComponent<LinkedSpriteManager>();
		
		sprite = sm.AddSprite(this.gameObject, 1.5f, 1.5f, sm.PixelCoordToUVCoord(7*96, 96), sm.PixelSpaceToUVSpace(96, 96), Vector3.zero, false);
		//sprite = sm.AddSprite(this.gameObject, 2f, 2f, 0, 48, 48, 48, false);
		walkRight = new UVAnimation();
		walkRight.BuildUVAnim(sm.PixelCoordToUVCoord(0, 96), sm.PixelSpaceToUVSpace(96, 96), 8, 1, 8, 10f);
		walkRight.loopCycles = -1;
		sprite.AddAnimation(walkRight);
		
		walkLeft = new UVAnimation();
		walkLeft.BuildUVAnim(sm.PixelCoordToUVCoord(0*96, 192), sm.PixelSpaceToUVSpace(96, 96), 8, 1, 8, 10f);
		walkLeft.loopCycles = -1;
		sprite.AddAnimation(walkLeft);
		
		standRight = new UVAnimation();
		standRight.BuildUVAnim(sm.PixelCoordToUVCoord(7*96, 96), sm.PixelSpaceToUVSpace(96, 96), 2, 1, 2, 1f);
		standRight.loopCycles = -1;
		sprite.AddAnimation(standRight);
		
		standLeft = new UVAnimation();
		standLeft.BuildUVAnim(sm.PixelCoordToUVCoord(7*96, 192), sm.PixelSpaceToUVSpace(96, 96), 2, 1, 2, 1f);
		standLeft.loopCycles = -1;
		sprite.AddAnimation(standLeft);
		
		attackRight = new UVAnimation();
		attackRight.BuildUVAnim(sm.PixelCoordToUVCoord(8*96, 96), sm.PixelSpaceToUVSpace(96, 96), 2, 1, 2, 4f);
		attackRight.loopCycles = 0;
		sprite.AddAnimation(attackRight);
		
		attackLeft = new UVAnimation();
		attackLeft.BuildUVAnim(sm.PixelCoordToUVCoord(8*96, 192), sm.PixelSpaceToUVSpace(96, 96), 2, 1, 2, 4f);
		attackLeft.loopCycles = 0;
		sprite.AddAnimation(attackLeft);
		
		state = (int) State.SEEKING;
	}
	
	void OnDestroy() {
		GameObject.Find("Spawner").GetComponent<C_Spawner>().signalCharDied();
        sm.RemoveSprite(sprite);
    }
	
	virtual protected void mainAttackAnim(M_Entity other) {
		Vector3 posDiff = other.transform.position - transform.position;
		float angle = Mathf.Atan2(posDiff.z, -posDiff.x) * Mathf.Rad2Deg;
		Quaternion animRot = Quaternion.Euler(new Vector3(0, 180+angle, 0));
		posDiff.y = transform.position.y;
		posDiff = transform.position + (posDiff.normalized * .4f);
		Instantiate(mainAttackEffect, other.transform.position, animRot);
	}
	
	virtual protected void attackMobIfNear() {
		if (Vector3.Distance(transform.position, targetMob.position) <= character.mainAttackRange) {
			state = (int) State.ATTACKING;
			charAI.canMove = false;
			Invoke("endMeleeCooldown", character.mainAttackCooldown);
			
			animateAttack(targetMob.GetComponent<M_Mob>());
			mainAttackAnim(targetMob.GetComponent<M_Mob>());
//			Vector3 direction = transform.position - targetMob.position;
//			direction.y = 0;
//			targetMob.rigidbody.AddForceAtPosition(-direction.normalized * 50f * character.meleePushback, targetMob.position);
			character.damage(targetMob.GetComponent<M_Mob>());
		}
	}
	
	virtual protected void attackBossIfNear() {
		if (Vector3.Distance(transform.position, targetBoss.position) <= character.mainAttackRange) {
			state = (int) State.ATTACKING;
			charAI.canMove = false;
			Invoke("endMeleeCooldown", character.mainAttackCooldown);
			
			animateAttack(targetBoss.GetComponent<M_Boss>());
			mainAttackAnim(targetBoss.GetComponent<M_Boss>());
			character.damage(targetBoss.GetComponent<M_Boss>());
		}
	}
	
	private void endMeleeCooldown() {
		charAI.canMove = true;
		state = (int) State.WALKING_TO_ATTACK;
	}
	
	// Update is called once per frame
	void Update () {
		animate();
		
		// BOSSFIGHT
		if (GameController.instance.gameState == (int) GameController.GameState.BOSSFIGHT) {
			if (state == (int) State.WALKING_TO_TAKE) {
				state = (int) State.SEEKING;
			} else if (state == (int) State.WALKING_TO_ATTACK) {
				if (targetBoss != null) {
					attackBossIfNear();
				}
			} else if (state == (int) State.SEEKING) {
				targetBoss = GameController.instance.boss.transform;
				charAI.target = targetBoss;
				charAI.canMove = true;
				charAI.targetSet = true;
				state = (int) State.WALKING_TO_ATTACK;
			}
			return;
		}
		
		// NORMAL
		if (state == (int) State.IDLE) {
			charAI.target = null;
			charAI.canMove = false;
			charAI.targetSet = false;
		} else if (state == (int) State.WALKING_TO_TAKE) {
			if (targetChest == null) {
				// Start Seeking
				state = (int) State.SEEKING;
			} else {
				if (!charAI.targetSet) {
					// Set Target
					charAI.target = targetChest;
					charAI.canMove = true;
					charAI.targetSet = true;
				}
			}
		} else if (state == (int) State.WALKING_TO_ATTACK) {
			if (targetMob == null) {
				// Search for new mob
				if (!targetNearMob()) {
					// Start Seeking
					state = (int) State.SEEKING;
				} else {
					// Set target next
					charAI.targetSet = false;
				}
			} else {
				if (!charAI.targetSet) {
					// Set Target
					charAI.target = targetMob;
					charAI.canMove = true;
					charAI.targetSet = true;
				} else {
					// Check to Attack
					attackMobIfNear();
				}
			}
		} else if (state == (int) State.SEEKING) {
			targetChest = findNearestObject("Chest", Mathf.Infinity);
			if (targetChest == null) {
				targetChest = GameObject.Find("Boss Door").transform;
				charAI.targetSet = false;
				// No chest, boss room
				state = (int) State.WALKING_TO_TAKE;
			} else {
				// Set target next
				charAI.targetSet = false;
				state = (int) State.WALKING_TO_TAKE;
			}
		}
	}
	
	public bool targetNearMob() {
		if (GameController.instance.gameState == (int) GameController.GameState.BOSSFIGHT) {
			return true;
		}
		targetMob = findNearestObject("Mob", mobAttentionRadius);
		if (targetMob && state != (int) State.ATTACKING) {
			state = (int) State.WALKING_TO_ATTACK;
			charAI.targetSet = false;
			return true;
		}
		return false;
	}
}
