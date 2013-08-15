using UnityEngine;
using System.Collections;

public class C_Mob : C_Entity {
	public float charAttentionRadius = 100f;
	public float wanderingDistance = 2f;
	
	protected EntityAI mobAI;
	protected M_Mob monster;
	protected Vector3 wanderCenter;
	protected Vector3 wanderPos;
	protected Transform targetChar;
	
	// Use this for initialization
	void Start () {
		monster = GetComponent<M_Mob>();
		mobAI = GetComponent<EntityAI>();
		initialize();
	}
	
	protected virtual void initialize() {
		wanderCenter = transform.position;
		InvokeRepeating("targetNearChar", 1f, 1f);
		InvokeRepeating("setWanderingPos", 0.0001f, 1f);
		
		sm = GameObject.Find("_SpriteManager").GetComponent<LinkedSpriteManager>();
		
		sprite = sm.AddSprite(this.gameObject, 1.5f, 1.5f, sm.PixelCoordToUVCoord(5*96, 288), sm.PixelSpaceToUVSpace(96, 96), Vector3.zero, false);
		//sprite = sm.AddSprite(this.gameObject, 2f, 2f, 0, 48, 48, 48, false);
		walkRight = new UVAnimation();
		walkRight.BuildUVAnim(sm.PixelCoordToUVCoord(0, 288), sm.PixelSpaceToUVSpace(96, 96), 6, 1, 6, 10f);
		walkRight.loopCycles = -1;
		sprite.AddAnimation(walkRight);
		
		walkLeft = new UVAnimation();
		walkLeft.BuildUVAnim(sm.PixelCoordToUVCoord(0*96, 384), sm.PixelSpaceToUVSpace(96, 96), 6, 1, 6, 10f);
		walkLeft.loopCycles = -1;
		sprite.AddAnimation(walkLeft);
		
		standRight = new UVAnimation();
		standRight.BuildUVAnim(sm.PixelCoordToUVCoord(5*96, 288), sm.PixelSpaceToUVSpace(96, 96), 2, 1, 2, 1f);
		standRight.loopCycles = -1;
		sprite.AddAnimation(standRight);
		
		standLeft = new UVAnimation();
		standLeft.BuildUVAnim(sm.PixelCoordToUVCoord(5*96, 384), sm.PixelSpaceToUVSpace(96, 96), 2, 1, 2, 1f);
		standLeft.loopCycles = -1;
		sprite.AddAnimation(standLeft);
		
		attackRight = new UVAnimation();
		attackRight.BuildUVAnim(sm.PixelCoordToUVCoord(6*96, 288), sm.PixelSpaceToUVSpace(96, 96), 2, 1, 2, 4f);
		attackRight.loopCycles = 0;
		sprite.AddAnimation(attackRight);
		
		attackLeft = new UVAnimation();
		attackLeft.BuildUVAnim(sm.PixelCoordToUVCoord(6*96, 384), sm.PixelSpaceToUVSpace(96, 96), 2, 1, 2, 4f);
		attackLeft.loopCycles = 0;
		sprite.AddAnimation(attackLeft);
		
		state = (int) State.SEEKING;
	}
	
	void OnDestroy() {
        sm.RemoveSprite(sprite);
    }
	
	protected virtual void attackCharIfNear() {
		if (Vector3.Distance(transform.position, targetChar.position) <= monster.mainAttackRange) {
			state = (int) State.ATTACKING;
			mobAI.canMove = false;
			Invoke("endMeleeCooldown", monster.mainAttackCooldown);
			
			animateAttack(targetChar.GetComponent<M_Char>());
			monster.damage(targetChar.GetComponent<M_Char>());
		}
	}
	
	protected void loseCharIfFar() {
		if (Vector3.Distance(transform.position, targetChar.position) > (charAttentionRadius + charAttentionRadius * .2f)) {
			mobAI.target = null;
			mobAI.targetSet = false;
			targetChar = null;
			wanderCenter = transform.position;
		}
	}
	
	protected void endMeleeCooldown() {
		mobAI.canMove = true;
		state = (int) State.WALKING_TO_ATTACK;
	}
	
	protected void wander() {
		Vector3 dir = (wanderPos - transform.position).normalized;
		dir *= mobAI.speed * Time.fixedDeltaTime * 100;
		rigidbody.AddForce(dir);
	}
	
	protected void setWanderingPos() {
		if (state == (int) State.SEEKING) {
			Vector2 wanderOffset = Random.insideUnitCircle * wanderingDistance;
			wanderPos = new Vector3(wanderCenter.x + wanderOffset.x, wanderCenter.y, wanderCenter.z + wanderOffset.y);
		}
	}
	
	
	void Update () {
		animate();
		
		if (state == (int) State.WALKING_TO_TAKE) {
			
		} else if (state == (int) State.WALKING_TO_ATTACK) {
			if (targetChar == null) {
				if (!targetNearChar()) {
					wanderCenter = transform.position;
					// Start Seeking
					mobAI.releasePath();
					state = (int) State.SEEKING;
				} else {
					// Set target next
					mobAI.targetSet = false;
				}
			} else {
				if (!mobAI.targetSet) {
					// Set Target
					mobAI.target = targetChar;
					mobAI.canMove = true;
					mobAI.targetSet = true;
				} else {
					// Check to Attack
					attackCharIfNear();
				}
			}
		}
	}
	
	void FixedUpdate() {
		if (state == (int) State.SEEKING) {
			wander();
		}
	}
	
	public bool targetNearChar() {
		if (targetChar != null) {
			loseCharIfFar();
			return false;
		}
		targetChar = findNearestObject("Char", charAttentionRadius);
		if (targetChar) {
			state = (int) State.WALKING_TO_ATTACK;
			mobAI.targetSet = false;
			return true;
		}
		return false;
	}
}
