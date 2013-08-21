using UnityEngine;
using System.Collections;

public class MainMenuInput : MonoBehaviour {
	public static MainMenuInput instance;
	
	public GameObject bossSelectWindow;
	
	
	void Start() {
		instance = this;
		DontDestroyOnLoad(GameObject.Find("_BossHolder"));
	}
	
	void Update () {
		if (Input.GetButtonDown("Fire1")) {
			UITooltip.ShowText("");
		}
	}
}
