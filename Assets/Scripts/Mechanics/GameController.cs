using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {
	public enum GameState {PREPARATION = 0, DEFENSE_CLEAR, DEFENSE_CIBR, BOSSFIGHT, GAMEOVER};
	// Game Options
	public int gameState = 0;
	public GameObject combatText;
	public int round = 1;
	public bool roundEnded = false;
	public static GameController instance;
	public float preparationTime = 10f;
	private float prepTime = 0f;
	private int charsOutOfBossRoom = 0;
	private Queue<GameObject> charsInBossRoom;
	public float allowedSpawnDistanceFromChar = 10f;
	public Dictionary<int,string> portraitSprites;
	public Dictionary<int,int> idToPrefab;
	public GameObject[] charPrefabs;
	public C_Boss bossC;
	public M_Boss bossM;
	public Vector3 bossSpawnPoint = new Vector3(15.9f, 0.5f, -35.8f);
	
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
	public GameObject boss;
	public GameObject incomingPanel;
	
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
		charsInBossRoom = new Queue<GameObject>();
	}
	
	void Start() {
		Screen.SetResolution(1280, 720, true);
		charsInRound = new int[] {1, 1, 1, 1, 1, 4, 4, 4, 4, 4};
		spawner = GameObject.Find("Spawner").GetComponent<C_Spawner>();
		foreach(Transform window in GameObject.Find("Window").transform) {
			NGUITools.SetActive(window.gameObject, false);
		}
		talentWindow.GetComponent<TalentWindow>().initialize();
		boss = Instantiate(BossHolder.instance.boss, bossSpawnPoint, BossHolder.instance.boss.transform.rotation) as GameObject;
		bossC = boss.GetComponent<C_Boss>();
		bossM = boss.GetComponent<M_Boss>();
		Bottombar.instance.l_bossLabel.text = bossM.eName;
		InvokeRepeating("increaseGrudge", 5f, 5f);
		Invoke("startNextDefense", 1f);
		gameState = (int)GameState.PREPARATION;
	}
	
	public void charEnteredBossRoom(GameObject enteredChar) {
		charsOutOfBossRoom--;
		enteredChar.GetComponent<C_Entity>().state = (int)C_Entity.State.IDLE;
		charsInBossRoom.Enqueue(enteredChar);
		
		checkToStartBossFight();
	}
	
	public void charDied() {
		charsOutOfBossRoom--;
		checkToStartBossFight();
	}
	
	public void checkToStartBossFight() {
		if (gameState == (int) GameState.BOSSFIGHT) return;
		if (charsOutOfBossRoom == 0 && charsInBossRoom.Count != 0) {
			while(charsInBossRoom.Count != 0) {
				GameObject charInBossRoom = charsInBossRoom.Dequeue();
				charInBossRoom.GetComponent<C_Entity>().state = (int)C_Entity.State.SEEKING;
			}
			gameState = (int)GameState.BOSSFIGHT;
			Bottombar.instance.switchToBossMode();
		}
	}
	
	public void endRound() {
		if (gameState == (int) GameState.DEFENSE_CLEAR) {
		} else if (gameState == (int) GameState.BOSSFIGHT) {
			Bottombar.instance.switchToNormalMode();
		}
		NGUITools.SetActive(incomingPanel, true);
		round++;
		gameState = (int)GameState.PREPARATION;
		prepTime = 0f;
		Invoke("startNextDefense", 1f);
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
		gameState = (int) GameState.GAMEOVER;
		NGUITools.SetActive(gameoverWindow, true);
		InputController.instance.inputLock = true;
	}
	
	public void startNextDefenseImmediately() {
		prepTime = preparationTime;
	}
	
	void startNextDefense() {
		if (prepTime < preparationTime) {
			prepTime += 1f;
			GameObject.Find("Incoming").GetComponent<UILabel>().text = "Chars incoming in " + (int) (preparationTime - prepTime) + " seconds...";
			Invoke("startNextDefense", 1f);
			return;
		}
		GameObject.Find("Incoming").GetComponent<UILabel>().text = "Chars incoming in 30 seconds...";
		gameState = (int)GameState.DEFENSE_CLEAR;
		NGUITools.SetActive(incomingPanel, false); // Incoming text
		if (round <= 10) {
			spawner.spawn(charsInRound[round-1]);
		} else {
			spawner.spawn();
		}
		charsOutOfBossRoom = spawner.charsLeft;
	}
	
	void increaseGrudge() {
		grudge += grudgePer5Seconds;
	}
	
}
