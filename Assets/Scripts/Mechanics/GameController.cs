using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {
	public enum GameState {PREPARATION = 0, DEFENSE_CLEAR, DEFENSE_CIBR, BOSSFIGHT};
	// Game Options
	public int gameState = 0;
	public GameObject combatText;
	public int round = 1;
	public bool roundEnded = false;
	public static GameController instance;
	public float preparationTime = 10f;
	private float prepTime = 0f;
	public float allowedSpawnDistanceFromChar = 10f;
	public Dictionary<int,string> portraitSprites;
	public Dictionary<int,int> idToPrefab;
	public GameObject[] charPrefabs;
	
	public int grudge = 1000;
	public int grudgePer5Seconds = 1;
	public int gold = 1000;
	public int level = 1;
	public int experience = 0;
	public int expRequired = 50;
	public int units = 0;
	public int maxUnits = 20;
	public int talentPoints = 0;
	
	public GameObject talentWindow;
	public GameObject gameoverWindow;
	public GameObject infoWindow;
	public GameObject HUD;
	public GameObject tinyWormPrefab;
	
	private C_Spawner spawner;
	
	private int[] charsInRound; // Shows ids of chars
	
	void Awake() {
		instance = this;
		if (portraitSprites == null) {
			portraitSprites = new Dictionary<int,string>();
			idToPrefab = new Dictionary<int,int>();
			portraitSprites.Add(1, "c_warrior_05");
			idToPrefab.Add(1,0);
			portraitSprites.Add(2, "m_worm_01");
			portraitSprites.Add(3, "m_skeleton_08");
			portraitSprites.Add(4, "c_mage_08");
			idToPrefab.Add(4,1);
		}
	}
	
	void Start() {
		Screen.SetResolution(1280, 720, true);
		charsInRound = new int[] {1, 1, 1, 1, 1, 4, 4, 4, 4, 4};
		spawner = GameObject.Find("Spawner").GetComponent<C_Spawner>();
		foreach(Transform window in GameObject.Find("Window").transform) {
			NGUITools.SetActive(window.gameObject, false);
		}
		talentWindow.GetComponent<TalentWindow>().initialize();
		InvokeRepeating("increaseGrudge", 5f, 5f);
		Invoke("startNextDefense", 1f);
	}
	
	public void endRound() {
		if (gameState == (int) GameState.DEFENSE_CLEAR) {
			GameObject.Find("Incoming").GetComponent<UILabel>().enabled = true; // Incoming text
			round++;
			gameState = (int)GameState.PREPARATION;
			prepTime = 0f;
			Invoke("startNextDefense", 1f);
		} else if (gameState == (int) GameState.DEFENSE_CIBR) {
			
		}
	}
	
	public void gainExp(int exp) {
		experience += exp;
		if (experience >= expRequired) {
			experience -= expRequired;
			level++;
			if(level > 10) gainTP(1);
		}
	}
	
	public void gainGold(int gld) {
		gold += gld;
	}
	
	public void gainTP(int tp) {
		talentPoints += tp;
	}
	
	public void gainGrudge(int anger) {
		grudge += anger;
	}
	
	public void spawnUnit() {
		units++;
	}
	
	public void despawnUnit() {
		units--;
	}
	
	public void gameOver() {
		NGUITools.SetActive(gameoverWindow, true);
		InputController.instance.inputLock = true;
	}
	
	void startNextDefense() {
		if (prepTime < preparationTime) {
			prepTime += 1f;
			GameObject.Find("Incoming").GetComponent<UILabel>().text = "Chars incoming in " + (int) (preparationTime - prepTime) + " seconds...";
			Invoke("startNextDefense", 1f);
			return;
		}
		GameObject.Find("Incoming").GetComponent<UILabel>().text = "Chars incoming in 10 seconds...";
		gameState = (int)GameState.DEFENSE_CLEAR;
		GameObject.Find("Incoming").GetComponent<UILabel>().enabled = false; // Incoming text
		if (round <= 10) {
			spawner.spawn(charsInRound[round-1]);
		} else {
			spawner.spawn();
		}
	}
	
	void increaseGrudge() {
		grudge += grudgePer5Seconds;
	}
	
}
