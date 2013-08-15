using UnityEngine;
using System.Collections;

public class E_Mageball : E_Projectile {
	
	UVAnimation dieAnim;
	
	override public void initialize(Vector3 dir, M_Entity sender) {
		Invoke("destroySelf", lifetime);
		entity = sender;
		direction = dir;
		
		sm = GameObject.Find("_SpriteManager").GetComponent<LinkedSpriteManager>();
		sprite = sm.AddSprite(this.gameObject, 1f, 1f, sm.PixelCoordToUVCoord(0, 12*96), sm.PixelSpaceToUVSpace(96, 96), Vector3.zero, false);
		sprite.SetDrawLayer(100);
		
		dieAnim = new UVAnimation();
		dieAnim.BuildUVAnim(sm.PixelCoordToUVCoord(0, 12*96), sm.PixelSpaceToUVSpace(96, 96), 4, 1, 4, 16f);
		dieAnim.loopCycles = 0;
		sprite.AddAnimation(dieAnim);
		
	}

	override protected void targetHit(GameObject other) {
		entity.damage(other.GetComponent<M_Entity>());
		destroySelf();
	}
	
	override protected void destroySelf() {
		sprite.PlayAnim(dieAnim);
		collider.enabled = false;
		speed = 0;
		Invoke("destroyNow", .25f);
		CancelInvoke("destroySelf");
	}
	
	protected void destroyNow() {
		sm.RemoveSprite(sprite);
		GameObject.Destroy(this.gameObject);
	}
}
