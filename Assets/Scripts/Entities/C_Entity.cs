using UnityEngine;
using System.Collections;

public class C_Entity : MonoBehaviour {
	public enum AnimState {STAND_RIGHT = 0, STAND_LEFT, WALK_RIGHT, WALK_LEFT, ATTACK_RIGHT, ATTACK_LEFT};
	public enum State {IDLE = 0, WALKING_TO_TAKE, WALKING_TO_ATTACK, ATTACKING, SEEKING};
	
	public int animState = 0;
	public int state = 0;
	public int standState = 0;
	public float standAnimationMaxSpeed = .3f;
	//public bool lockFacing = false; TODO
	
	protected LinkedSpriteManager sm;
	protected Sprite sprite;
	protected UVAnimation standRight;
	protected UVAnimation standLeft;
	protected UVAnimation walkRight;
	protected UVAnimation walkLeft;
	protected UVAnimation attackRight;
	protected UVAnimation attackLeft;
	
	public void animateAttack(MonoBehaviour other) {
		if (transform.position.x >= other.transform.position.x) {
			sprite.PlayAnim(attackLeft);
			animState = (int) AnimState.ATTACK_LEFT;
			standState = (int) AnimState.STAND_LEFT;
		} else {
			sprite.PlayAnim(attackRight);
			animState = (int) AnimState.ATTACK_RIGHT;
			standState = (int) AnimState.STAND_RIGHT;
		}
		Invoke("endAttackAnimation", .25f);
	}
	
	void OnDamage() {
		sprite.SetColor(Color.red);
		Invoke("resetColor", .1f);
	}
	
	public void resetColor() {
		sprite.SetColor(Color.white);
	}
	
	protected void endAttackAnimation() {
		animState = (int) AnimState.WALK_RIGHT;
	}

	protected void animate() {
		if (animState == (int) AnimState.ATTACK_RIGHT || animState == (int) AnimState.ATTACK_LEFT) {
			return;
		}
		if (Mathf.Abs(rigidbody.velocity.x) < standAnimationMaxSpeed && Mathf.Abs(rigidbody.velocity.z) < standAnimationMaxSpeed) {
			if (animState != (int) AnimState.STAND_RIGHT) {
				if (standState == (int) AnimState.STAND_RIGHT)
					sprite.PlayAnim(standRight);
				else
					sprite.PlayAnim(standLeft);
				animState = (int) AnimState.STAND_RIGHT;
			}
		} else if (rigidbody.velocity.x >= 0) {
			if (animState != (int) AnimState.WALK_RIGHT) {
				standState = (int) AnimState.STAND_RIGHT;
				sprite.PlayAnim(walkRight);
				animState = (int) AnimState.WALK_RIGHT;
			}
		} else {
			if (animState != (int) AnimState.WALK_LEFT) {
				standState = (int) AnimState.STAND_LEFT;
				sprite.PlayAnim(walkLeft);
				animState = (int) AnimState.WALK_LEFT;
			}
		}
	}
	
	public Transform findNearestObject(string tag, float maxRadius) {
		GameObject[] objs = GameObject.FindGameObjectsWithTag(tag);
		Transform foundObj = null;
		float distance = Mathf.Infinity;
		foreach(GameObject obj in objs) {
			Vector3 diff = obj.transform.position - transform.position;
            float curDistance = diff.sqrMagnitude;
			if (curDistance > maxRadius) continue;
            if (curDistance < distance) {
                foundObj = obj.transform;
                distance = curDistance;
            }
		}
		return foundObj;
	}
}
