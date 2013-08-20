using UnityEngine;
using System.Collections;

public class Spell : MonoBehaviour {
	
	[HideInInspector]
	public string sName;
	[HideInInspector]
	public string description;
	
	public int manaCost;
	public float cooldown;
	public int spellPower;
	public Transform crosshair;
	public string spriteName;
	
	protected C_Entity entityC;
	protected M_Entity entityM;
	
	/*---------------------------*/
	/*------Level Modifiers------*/
	/*---------------------------*/
	
	public float mod_manaCost = 10f;
	public float mod_spellPower = 1.5f;
	
	protected int getMod_manaCost() {
		int mManaCost = (int)(manaCost + GameController.instance.level * mod_manaCost);
		return mManaCost;
	}
	
	protected int getMod_spellPower() {
		int mSP = (int)(spellPower + GameController.instance.level * mod_spellPower);
		return mSP;
	}
	
	void levelAttributes() {
		manaCost = getMod_manaCost();
		spellPower = getMod_spellPower();
	}
	
	void Start() {
		entityC = GetComponent<C_Entity>();
		entityM = GetComponent<M_Entity>();
		levelAttributes();
		initialize();
	}
	
	virtual public void initialize() {
		
	}
	
	virtual public void checkCondition() {
		
	}
	
	public void spellCooldown() {}
	
	public string tooltipMessage() {
		string tooltipMessage;
		tooltipMessage = "[2EFFF1]" + sName + "[-]\n";
		tooltipMessage += "$mana " + manaCost + ", $cd " + cooldown + "s\n";
		tooltipMessage += "[CFCFCF]'" + description + "'[-]";
		return tooltipMessage;
	}
	
	public void showTooltip() {
		UITooltip.ShowText(tooltipMessage());
	}
}
