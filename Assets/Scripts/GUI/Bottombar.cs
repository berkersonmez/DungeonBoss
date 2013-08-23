using UnityEngine;
using System.Collections;

public class Bottombar : MonoBehaviour {
	
	public static Bottombar instance;
	public GameObject[] barMobs = new GameObject[5];
	public int mobsInBar = 0;
	public int selectedMob = 0;
	
	private GameObject chestPrefab;
	public bool chestSpawnMode = false;
	
	private UISprite s_selectBorder;
	private UISprite[] s_ports = new UISprite[5];
	private UILabel l_gold;
	private UILabel l_grudge;
	private UILabel l_level;
	private UILabel l_units;
	
	private UILabel l_health;
	private UILabel l_mana;
	private UISlider s_health;
	private UISlider s_mana;
	public UILabel l_bossLabel;
	
	void Awake() {
		instance = this;
		s_selectBorder = transform.Find("Panel").Find("Select").GetComponent<UISprite>();
		l_gold = transform.Find("Panel").Find("L_Gold").GetComponent<UILabel>();
		l_grudge = transform.Find("Panel").Find("L_Grudge").GetComponent<UILabel>();
		l_level = transform.Find("Panel").Find("L_Level").GetComponent<UILabel>();
		l_units = transform.Find("Panel").Find("L_Units").GetComponent<UILabel>();
		s_ports[0] = transform.Find("Panel").Find("Port1").GetComponent<UISprite>();
		s_ports[1] = transform.Find("Panel").Find("Port2").GetComponent<UISprite>();
		s_ports[2] = transform.Find("Panel").Find("Port3").GetComponent<UISprite>();
		s_ports[3] = transform.Find("Panel").Find("Port4").GetComponent<UISprite>();
		s_ports[4] = transform.Find("Panel").Find("Port5").GetComponent<UISprite>();
		l_health = transform.Find("Panel").Find("Healthbar Boss").Find("Boss Health").GetComponent<UILabel>();
		l_mana = transform.Find("Panel").Find("Manabar Boss").Find("Boss Mana").GetComponent<UILabel>();
		s_health = transform.Find("Panel").Find("Healthbar Boss").GetComponent<UISlider>();
		s_mana = transform.Find("Panel").Find("Manabar Boss").GetComponent<UISlider>();
		l_bossLabel = transform.Find("Panel").Find("Boss Label").GetComponent<UILabel>();
	}
	
	void Update() {
		l_gold.text = "$gold " + GameController.instance.gold;
		l_grudge.text = "$grudge " + GameController.instance.grudge;
		l_level.text = "$level " + GameController.instance.level;
		l_units.text = "$unit " + GameController.instance.units + "/" + GameController.instance.maxUnits;
		l_health.text = GameController.instance.bossM.health + "/" + GameController.instance.bossM.maxHealth;
		l_mana.text = GameController.instance.bossM.mana + "/" + GameController.instance.bossM.maxMana;
		s_health.sliderValue = (float) GameController.instance.bossM.health / GameController.instance.bossM.maxHealth;
		s_mana.sliderValue = (float) GameController.instance.bossM.mana / GameController.instance.bossM.maxMana;
	}
	
	public void addToBar(GameObject prefab) {
		barMobs[mobsInBar] = prefab;
		s_ports[mobsInBar].spriteName = GameController.instance.portraitSprites[prefab.GetComponent<M_Entity>().id];
		s_ports[mobsInBar].enabled = true;
		mobsInBar++;
	}
	
	public void switchToBossMode() {
		s_selectBorder.enabled = false;
		for (int i = 0 ; i < 5 ; i++) {
			if (GameController.instance.bossC.abilities[i] != null) {
				s_ports[i].enabled = true;
				s_ports[i].spriteName = GameController.instance.bossC.abilities[i].spriteName;
			} else {
				s_ports[i].enabled = false;
			}
		}
	}
	
	public void switchToNormalMode() {
		s_selectBorder.enabled = true;
		for (int i = 0 ; i < 5 ; i++) {
			if (barMobs[i] != null) {
				s_ports[i].spriteName = GameController.instance.portraitSprites[barMobs[i].GetComponent<M_Entity>().id];
				s_ports[i].enabled = true;
			} else {
				s_ports[i].enabled = false;
			}
		}
	}
	
	public void removeFromBar(GameObject prefab) {
		bool removed = false;
		for (int i = 0 ; i < 5 ; i++) {
			if (!removed && barMobs[i] == prefab) {
				removed = true;
				barMobs[i] = null;
				s_ports[i].enabled = false;
				continue;
			} else if (removed && barMobs[i] != null) {
				if (i-1 >= 0) {
					barMobs[i-1] = barMobs[i];
					s_ports[i-1].spriteName = s_ports[i].spriteName;
					s_ports[i-1].enabled = true;
				}
				barMobs[i] = null;
				s_ports[i].enabled = false;
			}
		}
		mobsInBar--;
	}
	
	public void selectMob(int index) {
		if (GameController.instance.gameState == (int) GameController.GameState.BOSSFIGHT) {
			if (index == 0) {
				GameController.instance.bossC.ability1();
			} else if (index == 1) {
				GameController.instance.bossC.ability2();
			} else if (index == 2) {
				GameController.instance.bossC.ability3();
			} else if (index == 3) {
				GameController.instance.bossC.ability4();
			} else if (index == 4) {
				GameController.instance.bossC.ability5();
			}
		} else {
			s_selectBorder.transform.localPosition = s_ports[index].transform.localPosition + new Vector3(-16, 16, 0);
			selectedMob = index;
		}
	}
	
	public void startChestSpawnMode(GameObject chestPrefab) {
		this.chestPrefab = chestPrefab;
		chestSpawnMode = true;
	}
	
	public void endChestSpawnMode() {
		this.chestPrefab = null;
		chestSpawnMode = false;
	}
	
	public void spawnMobReq(Vector3 position) {
		GameObject [] chars = GameObject.FindGameObjectsWithTag("Char");
		foreach(GameObject chr in chars) {
			if (Vector3.Distance(chr.transform.position, position) < GameController.instance.allowedSpawnDistanceFromChar) {
				return;
			}
		}
		if (s_ports[selectedMob].enabled && GameController.instance.units < GameController.instance.maxUnits) {
			barMobs[selectedMob].GetComponent<M_Mob>().spawn(position);
		}
	}
	
	public void spawnMob(Vector3 position) {
		GameController.instance.spawnUnit();
		Instantiate(barMobs[selectedMob], position + new Vector3(0, 0.2f, 0), barMobs[selectedMob].transform.rotation);
	}
	
	public GameObject spawnChest(Vector3 position) {
		if (GameController.instance.gold >= chestPrefab.GetComponent<C_Chest>().goldCost) {
			GameController.instance.gold -= chestPrefab.GetComponent<C_Chest>().goldCost;
			return Instantiate(chestPrefab, position + new Vector3(0, 0.2f, 0), chestPrefab.transform.rotation) as GameObject;
		}
		return null;
	}
}
