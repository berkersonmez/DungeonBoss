using UnityEngine;
using System.Collections;

public class M_Entity : MonoBehaviour {
	
	public int id;
	public int prefabId;
	public string eName;
	public string eDescription;
	
	public int health;
	public int mana;
	public int manaPerSecond;
	public int attackPower;
	public int defence;
	public int critChance;
	
	public float mainAttackRange;
	public float mainAttackCooldown;
	public float mainAttackPushback;
	
	public int maxMana;
	public int maxHealth;
	
	
	/*---------------------------*/
	/*------Level Modifiers------*/
	/*---------------------------*/
	
	public float mod_health = 125f;
	public float mod_mana = 10f;
	public float mod_manaPerSecond = .05f;
	public float mod_attackPower = 3f;
	public float mod_defence = .5f;
	public float mod_critChance = .1f;
	
	[HideInInspector]
	public HUDText HUD;
	[HideInInspector]
	public float poisonEndTime;
	[HideInInspector]
	public int poisonDamage;
	
	protected UISlider hudHealth;
	
	protected virtual int getMod_health() {
		int mHealth = (int)(health + mod_health * getLevel());
		return mHealth;
	}
	protected virtual int getMod_mana() {
		int mMana = (int)(mana + mod_mana * getLevel());
		return mMana;
	}
	protected virtual int getMod_manaPerSecond() {
		int mMPS = (int)(manaPerSecond + mod_manaPerSecond * getLevel());
		return mMPS;
	}
	protected virtual int getMod_attackPower() {
		int mAP = (int)(attackPower + mod_attackPower * getLevel());
		return mAP;
	}
	protected virtual int getMod_defence() {
		int mDefence = (int)(defence + mod_defence * getLevel());
		return Mathf.Clamp(mDefence, 0, 80);
	}
	protected virtual int getMod_critChance() {
		int mCrit = (int)(critChance + mod_critChance * getLevel());
		return Mathf.Clamp(mCrit, 0, 80);
	}
	protected virtual float getMod_mainAttackRange() {
		return mainAttackRange;
	}
	
	protected virtual int getLevel() {
		return GameController.instance.level;
	}
	
	protected virtual void levelAttributes() {
		health = getMod_health();
		mana = getMod_mana();
		manaPerSecond = getMod_manaPerSecond();
		attackPower = getMod_attackPower();
		defence = getMod_defence();
		critChance = getMod_critChance();
		mainAttackRange = getMod_mainAttackRange();
	}
	
	void Start() {
		levelAttributes();
		maxMana = mana;
		maxHealth = health;
		initialize();
		InvokeRepeating("increaseMana", 1f, 1f);
	}
	
	protected virtual void initialize() {
		GameObject hud = Instantiate(GameController.instance.HUD) as GameObject;
		hud.transform.parent = Camera.main.transform;
		hud.transform.localRotation = Quaternion.identity;
		hud.transform.localScale = new Vector3(1f, 1f, 1f);
		HUD = hud.GetComponent<HUDText>();
		HUD.GetComponent<UIFollowTarget>().target = this.transform;
		hudHealth = hud.transform.Find("Healthbar").GetComponent<UISlider>();
		HUD.transform.Find("Nickname").GetComponent<UILabel>().enabled = false;
		if (!InputController.instance.showDamages) {
			HUD.enabled = false;
		}
		if (!InputController.instance.showHealthbars) {
			UISprite foreground = HUD.transform.Find("Healthbar").Find("Foreground").GetComponent<UISprite>();
			UISprite background = HUD.transform.Find("Healthbar").Find("Background").GetComponent<UISprite>();
			foreground.enabled = false;
			background.enabled = false;
		}
	}
	
	void increaseMana() {
		mana += manaPerSecond;
		mana = Mathf.Clamp(mana, 0, maxMana);
	}
	
	public virtual void damage(M_Entity other) {
		int dmg = calculateDamage(other.defence, attackPower);
		other.receiveDamage(dmg);
		
		if (other.gameObject.tag == "Mob") {
			other.HUD.Add(-dmg, Color.red, .1f);
		} else if (other.gameObject.tag == "Char") {
			other.HUD.Add(-dmg, Color.yellow, .1f);
		} else if (other.gameObject.tag == "Boss") {
			other.HUD.Add(-dmg, Color.red, .1f);
		}
	}
	
	public void spellDamage(M_Entity other, int spellDamage) {
		int dmg = calculateDamage(other.defence, spellDamage);
		other.receiveDamage(dmg);
		
		if (other.gameObject.tag == "Mob") {
			other.HUD.Add("$sp" + (-dmg), Color.red, .1f);
		} else if (other.gameObject.tag == "Char") {
			other.HUD.Add("$sp" + (-dmg), Color.yellow, .1f);
		} else if (other.gameObject.tag == "Boss") {
			other.HUD.Add("$sp" + (-dmg), Color.red, .1f);
		}
	}
	
	public void getPoisoned(float duration, int tickDamage) {
		poisonDamage = Mathf.Max(tickDamage, poisonDamage);
		poisonEndTime = Time.time + duration;
		if (!IsInvoking("tickPoison")) {
			InvokeRepeating("tickPoison", 1f, 1f);
		}
	}
	
	protected void tickPoison() {
		if (Time.time > poisonEndTime) {
			CancelInvoke("tickPoison");
			return;
		} else {
			receivePoisonDamage(poisonDamage);
		}
	}
	
	public void receivePoisonDamage(int poisonDamage) {
		receiveDamage(poisonDamage);
		
		HUD.Add(-poisonDamage, Color.green, .1f);
	}
	
	public void receiveDamage(int dmg) {
		health -= dmg;
		SendMessage("OnDamage", dmg); 
	}
	
	public int calculateDamage(int otherDefence, int attackPower) {
		float damage = 0f;
		bool critical = Random.Range(0, 101) < critChance;
		
		damage = attackPower + Random.Range((float)-attackPower / 2, (float)attackPower / 2);
		if (critical) {
			damage *= 2;
		}
		
		damage -= (int)( damage * (otherDefence / 100f));
		return (int) damage;
	}
	
	public virtual void showTooltip() {
		
	}
	
}
