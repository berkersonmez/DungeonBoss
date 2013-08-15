using UnityEngine;
using System.Collections;

public class ShopSlot : MonoBehaviour {

	public GameObject prefab;
	public bool bought;
	public bool selected;
	public bool locked;
	public int unlocksAt;
	
	private UISprite s_avatar;
	private UILabel l_description;
	private UISprite s_button;
	private UILabel l_button;
	private UISprite s_selected;
	
	void Start() {
		s_avatar = transform.Find("Avatar").GetComponent<UISprite>();
		s_button = transform.Find("Button").GetComponent<UISprite>();
		l_description = transform.Find("Description").GetComponent<UILabel>();
		l_button = transform.Find("L_Button").GetComponent<UILabel>();
		s_selected = transform.Find("Selected").GetComponent<UISprite>();
		if (!selected) {
			s_selected.enabled = false;
		}
		M_Mob mob = prefab.GetComponent<M_Mob>();
		l_description.text = "'" + mob.eDescription + "'";
		if (locked) {
			s_button.spriteName = "ui_mobselect_locked";
			l_button.text = "[FFFFFF]Unlock at $level [00FE21]" + unlocksAt;
		} else if (bought) {
			s_button.spriteName = "ui_mobselect_talent";
			l_button.text = "[FFFFFF]Talents";
		} else {
			l_button.text = "Buy $gold" + mob.goldCost;
		}
	}
	
	public void unlock() {
		s_button.spriteName = "ui_mobselect_buy";
		l_button.text = "Buy $gold" + prefab.GetComponent<M_Mob>().goldCost;
		locked = false;
	}
	
	public void changeButtonToTalent() {
		bought = true;
		s_button.spriteName = "ui_mobselect_talent";
		l_button.text = "[FFFFFF]Talents";
	}
	
	public void toggleSelected() {
		if (selected) {
			selected = false;
			s_selected.enabled = false;
		} else {
			selected = true;
			s_selected.enabled = true;
		}
	}
}
