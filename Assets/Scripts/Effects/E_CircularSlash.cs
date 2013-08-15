using UnityEngine;
using System.Collections;

public class E_CircularSlash : MonoBehaviour {

	protected Sprite sprite;
	protected LinkedSpriteManager sm;
	
	// Use this for initialization
	void Start () {
		sm = GameObject.Find("_SpriteManager").GetComponent<LinkedSpriteManager>();
		sprite = sm.AddSprite(this.gameObject, 1f, 1f, sm.PixelCoordToUVCoord(0, 9*96), sm.PixelSpaceToUVSpace(192, 192), Vector3.zero, false);
		sprite.SetDrawLayer(100);
		UVAnimation anim = new UVAnimation();
		anim.BuildUVAnim(sm.PixelCoordToUVCoord(0, 9*96), sm.PixelSpaceToUVSpace(192, 192), 5, 1, 5, 56f);
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
