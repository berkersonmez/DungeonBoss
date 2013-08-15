using UnityEngine;
using System.Collections;

public class CombatText : MonoBehaviour {
	public enum HitType {MOB = 0, CHAR};
	
	public Material mobHit;
	public Material charHit;
	
	Vector2 speed;
	float gravity = 12f;
	
	void Start() {
		Invoke("destroySelf", .5f);
		speed = new Vector2(0, 0);
		speed.x = Random.Range(-.5f, .5f);
		speed.y = Random.Range(2f, 3f);
	}

	public void setText(string text, int hitType) {
		TextMesh tm = GetComponent<TextMesh>();
		tm.text = text;
		if (hitType == (int) HitType.CHAR) {
			tm.renderer.material = charHit;
		} else if (hitType == (int) HitType.MOB) {
			tm.renderer.material = mobHit;
		}
	}
	
	void destroySelf() {
		Destroy(gameObject);
	}
	
	void Update() {
		transform.position += new Vector3(speed.x, 0, speed.y) * Time.deltaTime;
		speed.y -= gravity * Time.deltaTime;
	}
}
