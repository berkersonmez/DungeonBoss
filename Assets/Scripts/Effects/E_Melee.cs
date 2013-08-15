using UnityEngine;
using System.Collections;

public class E_Melee : MonoBehaviour {
	
	protected Sprite sprite;
	protected LinkedSpriteManager sm;
	
	// Use this for initialization
	void Start () {
		sm = GameObject.Find("_SpriteManager").GetComponent<LinkedSpriteManager>();
		sprite = sm.AddSprite(this.gameObject, 1f, 1f, sm.PixelCoordToUVCoord(0, 5*96), sm.PixelSpaceToUVSpace(96, 96), Vector3.zero, false);
		sprite.SetDrawLayer(100);
		UVAnimation anim = new UVAnimation();
		anim.BuildUVAnim(sm.PixelCoordToUVCoord(0, 5*96), sm.PixelSpaceToUVSpace(96, 96), 7, 1, 7, 56f);
		anim.loopCycles = 0;
		sprite.AddAnimation(anim);
		sprite.PlayAnim(anim);
		Invoke("destroySelf", .125f);
	}
	
	
	void destroySelf() {
		sm.RemoveSprite(sprite);
		GameObject.Destroy(this.gameObject);
	}
}
