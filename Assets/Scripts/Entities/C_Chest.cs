using UnityEngine;
using System.Collections;

public class C_Chest : MonoBehaviour {

	int grudgeCost = 30;
	protected SpriteManager sm;
	protected Sprite sprite;
	
	void Start() {
		sm = GameObject.Find("_SpriteManager").GetComponent<LinkedSpriteManager>();
		
		sprite = sm.AddSprite(this.gameObject, 1.5f, 1.5f, sm.PixelCoordToUVCoord(8*96, 288), sm.PixelSpaceToUVSpace(96, 96), Vector3.zero, false);
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Char") {
			GameController.instance.gainGrudge(grudgeCost / 2);
			sm.RemoveSprite(sprite);
			Destroy(this.gameObject);
		}
    }
	
	
}
