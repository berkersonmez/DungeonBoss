using UnityEngine;
using System.Collections;

public class C_Boss : C_Entity {
	public float speed = 5;
	
	protected M_Boss boss;
	protected Vector3 moveDirection;

	void Start () {
		boss = GetComponent<M_Boss>();
		initialize();
	}
	
	protected virtual void initialize() {
		rigidbody.freezeRotation = true;
		sm = GameObject.Find("_SpriteManager2").GetComponent<LinkedSpriteManager>();
		
		sprite = sm.AddSprite(this.gameObject, 1.5f, 1.5f, sm.PixelCoordToUVCoord(7*192, 192), sm.PixelSpaceToUVSpace(192, 192), Vector3.zero, false);
		
		walkRight = new UVAnimation();
		walkRight.BuildUVAnim(sm.PixelCoordToUVCoord(0, 192), sm.PixelSpaceToUVSpace(192, 192), 8, 1, 8, 10f);
		walkRight.loopCycles = -1;
		sprite.AddAnimation(walkRight);
		
		walkLeft = new UVAnimation();
		walkLeft.BuildUVAnim(sm.PixelCoordToUVCoord(0*192, 384), sm.PixelSpaceToUVSpace(192, 192), 8, 1, 8, 10f);
		walkLeft.loopCycles = -1;
		sprite.AddAnimation(walkLeft);
		
		standRight = new UVAnimation();
		standRight.BuildUVAnim(sm.PixelCoordToUVCoord(7*192, 192), sm.PixelSpaceToUVSpace(192, 192), 2, 1, 2, 1f);
		standRight.loopCycles = -1;
		sprite.AddAnimation(standRight);
		
		standLeft = new UVAnimation();
		standLeft.BuildUVAnim(sm.PixelCoordToUVCoord(7*192, 384), sm.PixelSpaceToUVSpace(192, 192), 2, 1, 2, 1f);
		standLeft.loopCycles = -1;
		sprite.AddAnimation(standLeft);
		
		attackRight = new UVAnimation();
		attackRight.BuildUVAnim(sm.PixelCoordToUVCoord(8*192, 192), sm.PixelSpaceToUVSpace(192, 192), 2, 1, 2, 4f);
		attackRight.loopCycles = 0;
		sprite.AddAnimation(attackRight);
		
		attackLeft = new UVAnimation();
		attackLeft.BuildUVAnim(sm.PixelCoordToUVCoord(8*192, 384), sm.PixelSpaceToUVSpace(192, 192), 2, 1, 2, 4f);
		attackLeft.loopCycles = 0;
		sprite.AddAnimation(attackLeft);
	}
	
	void OnDestroy() {
        sm.RemoveSprite(sprite);
    }
	
	public virtual void move(Vector3 moveDir) {
		moveDirection = moveDir.normalized;
	}
	
	void Update () {
		animate();
	}
	
	void FixedUpdate () {
		rigidbody.AddForce(moveDirection * speed * Time.fixedDeltaTime * 100);
		
	}
}
