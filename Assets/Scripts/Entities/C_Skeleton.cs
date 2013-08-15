using UnityEngine;
using System.Collections;

public class C_Skeleton : C_Mob {
	
	protected override void initialize() {
		wanderCenter = transform.position;
		InvokeRepeating("targetNearChar", 1f, 1f);
		InvokeRepeating("setWanderingPos", 0.0001f, 1f);
		
		sm = GameObject.Find("_SpriteManager").GetComponent<LinkedSpriteManager>();
		
		sprite = sm.AddSprite(this.gameObject, 1.5f, 1.5f, sm.PixelCoordToUVCoord(7*96, 6*96), sm.PixelSpaceToUVSpace(96, 96), Vector3.zero, false);
		//sprite = sm.AddSprite(this.gameObject, 2f, 2f, 0, 48, 48, 48, false);
		walkRight = new UVAnimation();
		walkRight.BuildUVAnim(sm.PixelCoordToUVCoord(0, 6*96), sm.PixelSpaceToUVSpace(96, 96), 8, 1, 8, 10f);
		walkRight.loopCycles = -1;
		sprite.AddAnimation(walkRight);
		
		walkLeft = new UVAnimation();
		walkLeft.BuildUVAnim(sm.PixelCoordToUVCoord(0*96, 7*96), sm.PixelSpaceToUVSpace(96, 96), 8, 1, 8, 10f);
		walkLeft.loopCycles = -1;
		sprite.AddAnimation(walkLeft);
		
		standRight = new UVAnimation();
		standRight.BuildUVAnim(sm.PixelCoordToUVCoord(7*96, 6*96), sm.PixelSpaceToUVSpace(96, 96), 2, 1, 2, 1f);
		standRight.loopCycles = -1;
		sprite.AddAnimation(standRight);
		
		standLeft = new UVAnimation();
		standLeft.BuildUVAnim(sm.PixelCoordToUVCoord(7*96, 7*96), sm.PixelSpaceToUVSpace(96, 96), 2, 1, 2, 1f);
		standLeft.loopCycles = -1;
		sprite.AddAnimation(standLeft);
		
		attackRight = new UVAnimation();
		attackRight.BuildUVAnim(sm.PixelCoordToUVCoord(8*96, 6*96), sm.PixelSpaceToUVSpace(96, 96), 2, 1, 2, 4f);
		attackRight.loopCycles = 0;
		sprite.AddAnimation(attackRight);
		
		attackLeft = new UVAnimation();
		attackLeft.BuildUVAnim(sm.PixelCoordToUVCoord(8*96, 7*96), sm.PixelSpaceToUVSpace(96, 96), 2, 1, 2, 4f);
		attackLeft.loopCycles = 0;
		sprite.AddAnimation(attackLeft);
		
		state = (int) State.SEEKING;
	}
}
