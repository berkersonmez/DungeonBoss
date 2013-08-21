using UnityEngine;
using System.Collections;

public class BossHolder : MonoBehaviour {
	public static BossHolder instance;
	
	public GameObject boss;
	
	void Start () {
		instance = this;
	}
	
}
