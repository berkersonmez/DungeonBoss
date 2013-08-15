using UnityEngine;
using System.Collections;

public class E_Projectile : MonoBehaviour {

	public float speed;
	protected Vector3 direction;
	public bool spectral = false;
	public float lifetime;
	public string targetTag = "Mob";
	
	protected M_Entity entity;	
	protected Sprite sprite;
	protected LinkedSpriteManager sm;
		
	virtual public void initialize(Vector3 dir, M_Entity sender) {
		Invoke("destroySelf", lifetime);
		entity = sender;
		direction = dir;
	}
	
	void FixedUpdate() {
		move();
	}
	
	virtual protected void move() {
		transform.position += direction * speed;
	}
	
	void OnTriggerEnter(Collider other) {
		if (!spectral && other.gameObject.tag == "Wall") {
			destroySelf();
		} else if (other.gameObject.tag == targetTag) {
			targetHit(other.gameObject);
		}
    }
	
	virtual protected void targetHit(GameObject target) {
		
	}
	
	virtual protected void destroySelf() {
		
	}
	
	
}
