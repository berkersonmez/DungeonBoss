using UnityEngine;
using System;
using System.Collections;
using System.Reflection;

public class TalentWindow : MonoBehaviour {
	public static TalentWindow instance;
	public M_Mob talentOwner;
	public TalentSlot [] slots;
	
	/* Talents */
	public Talent [] talents_2; // Worm
	public Talent [] talents_3; // Skeleton
	
	private UILabel tpText;
	
	public void initialize() {
		instance = this;
		tpText = transform.Find("TP").GetComponent<UILabel>();
		setTalents();
	}
	
	void Update() {
		tpText.text = "$tp " + GameController.instance.talentPoints;
	}
	
	public void openWindow(int mobID) {
		string tName = "talents_" + mobID;
		Array a = (Array) this.GetType().GetField(tName).GetValue(this);
		for(int i = 0 ; i < a.Length ; i++) {
			Talent talent = (Talent) a.GetValue(i);
			slots[i].setTalent(talent);
		}
	}
	
	public bool checkRequisite(int slotID) {
		if (slotID % 3 != 0 && !slots[slotID - 1].talent.active) {
			return false;
		}
		return true;
	}
	
	public bool isActive(int mobID, int slotID) {
		string tName = "talents_" + mobID;
		Array a = (Array) this.GetType().GetField(tName).GetValue(this);
		Talent talent = (Talent) a.GetValue(slotID);
		return talent.active;
	}
	
	void setTalents() {
		// WORM
		talents_2 = new Talent[9];
		talents_2[0] = new Talent(0, "Double Trouble", 
			"Worms split into two tiny worms on death. Tiny worms have 25% health.",
			"t_worm_1");
		talents_2[1] = new Talent(1, "Bad Gas", 
			"Tiny worms poison nearby enemies on death for 10 seconds, 10 damage per second." +
			 " Poison does not stack, but refreshes.",
			"t_worm_2");
		talents_2[2] = new Talent(2, "Double Double Trouble", 
			"Tiny worms have 50% health instead of 25%.",
			"t_worm_3");
		talents_2[3] = new Talent(3, "Halitosis", 
			"Attacks may poison enemy for 5 seconds, 10 damage per second. Poison does not stack, but refreshes.",
			"t_worm_4");
		talents_2[4] = new Talent(4, "Clinging Odor", 
			"Increase poison duration for 5 seconds.",
			"t_worm_5");
		talents_2[5] = new Talent(5, "Pullulate", 
			"Decrease grudge cost by 1 and decrease maximum health by 30%.",
			"t_worm_6");
		talents_2[6] = new Talent(6, "Thick Skin", 
			"Increase defense by 5.",
			"t_worm_7");
		talents_2[7] = new Talent(7, "Thicker Skin", 
			"Increase defense by 5.",
			"t_worm_8");
		talents_2[8] = new Talent(8, "Regeneration", 
			"Regenerate 1% health per second.",
			"t_worm_9");
		
		// SKELETON
		talents_3 = new Talent[9];
		talents_3[0] = new Talent(0, "United", 
			"Increase attack power by 5% for each nearby skeleton.",
			"t_skeleton_1");
		talents_3[1] = new Talent(1, "Empowered", 
			"Increase base attack power by 5%.",
			"t_skeleton_2");
		talents_3[2] = new Talent(2, "Pacified", 
			"Decrease grudge cost by 2.",
			"t_skeleton_3");
		talents_3[3] = new Talent(3, "Spiny Sword", 
			"Increase critical chance by 10%.",
			"t_skeleton_4");
		talents_3[4] = new Talent(4, "Slendy", 
			"Increase melee range by 50%.",
			"t_skeleton_5");
		talents_3[5] = new Talent(5, "Fast Learner", 
			"Skeletons advance 5 levels ahead.",
			"t_skeleton_6");
		talents_3[6] = new Talent(6, "Merry-Go-Round", 
			"Learn 'Circular Slash'.",
			"t_skeleton_7");
		talents_3[7] = new Talent(7, "Hard Hitter", 
			"Increase spell power of 'Circular Slash' by 10%.",
			"t_skeleton_8");
		talents_3[8] = new Talent(8, "Ferris Wheel", 
			"Increase range of 'Circular Slash' by 50%.",
			"t_skeleton_9");
	}
}
