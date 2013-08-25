using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class SaveController : MonoBehaviour {
	
	public static SaveController instance;
	
	[System.Serializable]
	public class SaveGame {
		public int level;
		public int round;
		public int grudge;
		public int grudgePer5Seconds;
		public int gold;
		public int experience;
		public int expRequired;
		public int maxUnits;
		public int talentPoints;
		
		public int bossPrefabID;
		public int bossHealth;
		public int bossMana;
		public int bossMaxHealth;
		public int bossMaxMana;
		
		public List<int[]> mobs;
		public List<float[]> mobsPos;
		
		public List<int[]> rooms;
		
		public List<int[]> chests;
		public List<float[]> chestPos;
		
		public List<int[]> spawnSlots;
		
		public List<int[]> talents;
		
		public string saveTime;
	}
	
	public GameObject saveSlotPrefab;
	[HideInInspector]
	public bool newGame;
	[HideInInspector]
	public int saveID;
	public static SaveGame savedGame;
	
	private List<M_Mob> instantiatedMobs;

	void Start () {
		instance = this;
		newGame = true;
	}
	
	public void startNewSave() {
		int i = 1;
		while (PlayerPrefs.HasKey("sv" + i)) {
			i++;
		}
		saveID = i;
	}
	
	public void saveLevel() {
		prepareLevelToBeSaved();
		save("sv"+saveID);
	}
	
	public SaveGame loadLevel(int saveID) {
		return load("sv"+saveID);
	}
	
	public void setToLoad(SaveGame saveGame, int id) {
		savedGame = saveGame;
		saveID = id;
		newGame = false;
	}
	
	public void prepareSaveSelectWindow() {
		GameObject ssWindow = GameObject.Find("Save Select");
		int i = 0;
		while (true) {
			i++;
			if (PlayerPrefs.HasKey("sv" + i)) {
				SaveGame sg = loadLevel(i);
				GameObject saveSlot = Instantiate(saveSlotPrefab) as GameObject;
				saveSlot.transform.parent = ssWindow.transform.Find("Scroller").Find("Grid");
				saveSlot.transform.localScale = new Vector3(.4f, .4f, 1f);
				saveSlot.GetComponent<SaveSlot>().setSaveGame(sg, i);
			} else {
				break;
			}
		}
	}
	
	public void prepareLevelToBeSaved() {
		savedGame = new SaveGame();
		savedGame.level = GameController.instance.level;
		savedGame.round = GameController.instance.round;
		savedGame.grudge = GameController.instance.grudge;
		savedGame.grudgePer5Seconds = GameController.instance.grudgePer5Seconds;
		savedGame.gold = GameController.instance.gold;
		savedGame.experience = GameController.instance.experience;
		savedGame.expRequired = GameController.instance.expRequired;
		savedGame.maxUnits = GameController.instance.maxUnits;
		savedGame.talentPoints = GameController.instance.talentPoints;
		savedGame.saveTime = System.DateTime.Now.ToString();
		savedGame.bossHealth = GameController.instance.bossM.health;
		savedGame.bossMana = GameController.instance.bossM.mana;
		savedGame.bossMaxHealth = GameController.instance.bossM.maxHealth;
		savedGame.bossMaxMana = GameController.instance.bossM.maxMana;
		savedGame.bossPrefabID = GameController.instance.bossM.prefabId;
		
		savedGame.mobs = new List<int[]>();
		savedGame.mobsPos = new List<float[]>();
		M_Mob[] mobList = FindObjectsOfType(typeof(M_Mob)) as M_Mob[];
		int i = 0;
		foreach (M_Mob mob in mobList) {
			savedGame.mobs.Add(new int[5]);
			savedGame.mobs[i][0] = mob.prefabId;
			savedGame.mobs[i][1] = mob.health;
			savedGame.mobs[i][2] = mob.mana;
			savedGame.mobs[i][3] = mob.maxHealth;
			savedGame.mobs[i][4] = mob.maxMana;
			
			savedGame.mobsPos.Add(new float[3]);
			savedGame.mobsPos[i][0] = mob.transform.position.x;
			savedGame.mobsPos[i][1] = mob.transform.position.y;
			savedGame.mobsPos[i][2] = mob.transform.position.z;
			i++;
		}
		
		savedGame.rooms = new List<int[]>();
		GameObject[] roomList = GameController.instance.roomShades;
		i = 0;
		foreach (GameObject room in roomList) {
			RoomController roomCtrl = room.GetComponent<RoomController>();
			savedGame.rooms.Add(new int[2]);
			savedGame.rooms[i][0] = roomCtrl.id;
			savedGame.rooms[i][1] = (roomCtrl.locked ? 1 : 0);
			i++;
		}
		
		savedGame.chests = new List<int[]>();
		savedGame.chestPos = new List<float[]>();
		C_Chest[] chestList = FindObjectsOfType(typeof(C_Chest)) as C_Chest[];
		i = 0;
		foreach (C_Chest chest in chestList) {
			savedGame.chests.Add(new int[2]);
			savedGame.chests[i][0] = chest.prefabId;
			savedGame.chests[i][1] = chest.roomID;
			
			savedGame.chestPos.Add(new float[3]);
			savedGame.chestPos[i][0] = chest.transform.position.x;
			savedGame.chestPos[i][1] = chest.transform.position.y;
			savedGame.chestPos[i][2] = chest.transform.position.z;
			i++;
		}
		
		// Spawn Menu
		savedGame.spawnSlots = new List<int[]>();
		i = 0;
		while (true) {
			i++;
			Transform mobSlot = GameController.instance.mobWindow.transform.Find("Scroller/Grid/Slot" + i);
			if (mobSlot != null) {
				ShopSlot sSlot = mobSlot.GetComponent<ShopSlot>();
				savedGame.spawnSlots.Add(new int[4]);
				savedGame.spawnSlots[i-1][0] = (sSlot.locked ? 1 : 0);
				savedGame.spawnSlots[i-1][1] = (sSlot.bought ? 1 : 0);
				savedGame.spawnSlots[i-1][2] = (sSlot.selected ? 1 : 0);
				int prefabId = sSlot.prefab.GetComponent<M_Entity>() == null ? sSlot.prefab.GetComponent<C_Chest>().prefabId : sSlot.prefab.GetComponent<M_Entity>().prefabId;
				savedGame.spawnSlots[i-1][3] = prefabId;
			} else {
				break;
			}
		}
		
		// Talents
		savedGame.talents = new List<int[]>();
		i = 0;
		foreach(Talent[] talents in TalentWindow.instance.allTalents) {
			savedGame.talents.Add(new int[9]);
			for (int j = 0 ; j < 9 ; j++) {
				savedGame.talents[i][j] = (talents[j].active ? 1 : 0);
			}
			i++;
		}
	}
	
	public void cleanLevelForLoad() {
		foreach(GameObject cObj in GameObject.FindGameObjectsWithTag("Chest")) {
			cObj.GetComponent<C_Chest>().destroyNow();
		}
	}
	
	public void prepareLoadedLevel() {
		NGUITools.SetActive(GameController.instance.loadingWindow, true);
		GameController.instance.level = savedGame.level;
		GameController.instance.round = savedGame.round;
		GameController.instance.grudge = savedGame.grudge;
		GameController.instance.grudgePer5Seconds = savedGame.grudgePer5Seconds;
		GameController.instance.gold = savedGame.gold;
		GameController.instance.experience = savedGame.experience;
		GameController.instance.expRequired = savedGame.expRequired;
		GameController.instance.maxUnits = savedGame.maxUnits;
		GameController.instance.talentPoints = savedGame.talentPoints;
		
		GameObject bossPrefab = PrefabIndex.instance.prefabIndex[savedGame.bossPrefabID];
		GameController.instance.boss = Instantiate(bossPrefab, GameController.instance.bossSpawnPoint, bossPrefab.transform.rotation) as GameObject;
		
		instantiatedMobs = new List<M_Mob>();
		int i = 0;
		foreach (int[] mobAttr in savedGame.mobs) {
			GameObject mobPrefab = PrefabIndex.instance.prefabIndex[savedGame.mobs[i][0]];
			Vector3 mobPos = new Vector3(savedGame.mobsPos[i][0], savedGame.mobsPos[i][1], savedGame.mobsPos[i][2]);
			GameObject insMob = Instantiate(mobPrefab, mobPos, mobPrefab.transform.rotation) as GameObject;
			if (insMob.GetComponent<M_Mob>().eName != "Tiny Worm") {
				GameController.instance.spawnUnit();
			}
			instantiatedMobs.Add(insMob.GetComponent<M_Mob>());
			i++;
		}
		
		foreach (int[] roomAttr in savedGame.rooms) {
			GameController.instance.roomShades[roomAttr[0]].GetComponent<RoomController>().locked = (roomAttr[1] == 1);
		}
		
		NGUITools.SetActive(GameController.instance.mobWindow, true);
		
		Invoke("postPrepareLoadedLevel", .5f);
	}
	
	void postPrepareLoadedLevel() {
		cleanLevelForLoad();
		M_Boss bossM = GameController.instance.boss.GetComponent<M_Boss>();
		bossM.health = savedGame.bossHealth;
		bossM.mana = savedGame.bossMana;
		bossM.maxHealth = savedGame.bossMaxHealth;
		bossM.maxMana = savedGame.bossMaxMana;
		
		int i = 0;
		foreach (M_Mob mob in instantiatedMobs) {
			mob.health = savedGame.mobs[i][1];
			mob.mana = savedGame.mobs[i][2];
			mob.maxHealth = savedGame.mobs[i][3];
			mob.maxMana = savedGame.mobs[i][4];
			i++;
		}
		
		i = 0;
		foreach (float[] chestAttr in savedGame.chestPos) {
			GameObject chestPrefab = PrefabIndex.instance.prefabIndex[savedGame.chests[i][0]];
			Vector3 chestPos = new Vector3(savedGame.chestPos[i][0], savedGame.chestPos[i][1], savedGame.chestPos[i][2]);
			GameObject insChest = Instantiate(chestPrefab, chestPos, chestPrefab.transform.rotation) as GameObject;
			GameController.instance.roomShades[savedGame.chests[i][1]].GetComponent<RoomController>().addChest(insChest);
			i++;
		}
		
		// Spawn Menu
		i = 0;
		while (true) {
			i++;
			Transform mobSlot = GameController.instance.mobWindow.transform.Find("Scroller/Grid/Slot" + i);
			if (mobSlot != null) {
				ShopSlot sSlot = mobSlot.GetComponent<ShopSlot>();
				if (savedGame.spawnSlots[i-1][0] != 1) {
					sSlot.unlock();
				}
				if (savedGame.spawnSlots[i-1][1] == 1) {
					sSlot.changeButtonToTalent();
				}
				if (savedGame.spawnSlots[i-1][2] == 1) {
					sSlot.toggleSelected();
					Bottombar.instance.addToBar(PrefabIndex.instance.prefabIndex[savedGame.spawnSlots[i-1][3]]);
				}
			} else {
				break;
			}
		}
		NGUITools.SetActive(GameController.instance.mobWindow, false);
		
		// Talents
		i = 0;
		foreach(Talent[] talents in TalentWindow.instance.allTalents) {
			for (int j = 0 ; j < 9 ; j++) {
				talents[j].active = savedGame.talents[i][j] == 1;
			}
			i++;
		}
		
		NGUITools.SetActive(GameController.instance.loadingWindow, false);
	}
	
	private void save(string prefKey) {
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(SaveGame));
		StringWriter stringWriter = new StringWriter();
		
		xmlSerializer.Serialize(stringWriter, savedGame);
		PlayerPrefs.SetString(prefKey, stringWriter.ToString());
	}
	
	private SaveGame load(string prefKey) {
		if (!PlayerPrefs.HasKey(prefKey)) {
			return null;
		}
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(SaveGame));
		StringReader stringReader = new StringReader(PlayerPrefs.GetString(prefKey));
		return (SaveGame)xmlSerializer.Deserialize(stringReader);
	}
	
}
