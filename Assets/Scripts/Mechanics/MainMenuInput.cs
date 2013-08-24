using UnityEngine;
using System.Collections;

public class MainMenuInput : MonoBehaviour {
	public static MainMenuInput instance;
	
	public GameObject bossSelectWindow;
	public GameObject saveSelectWindow;
	
	
	void Start() {
		instance = this;
		DontDestroyOnLoad(GameObject.Find("_BossHolder"));
		DontDestroyOnLoad(GameObject.Find("_SaveController"));
	}
	
	void Update () {
		if (Input.GetButtonDown("Fire1")) {
			UITooltip.ShowText("");
		}
	}
}
