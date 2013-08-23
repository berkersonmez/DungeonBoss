using UnityEngine;
using System.Collections;

public class ShopChestSlot : MonoBehaviour {

	public GameObject prefab;
	public bool locked;
	public int unlocksAt;
	
	private UISprite s_avatar;
	private UILabel l_description;
	private UISprite s_button;
	private UILabel l_button;
	
	void Start() {
		s_avatar = transform.Find("Avatar").GetComponent<UISprite>();
		s_button = transform.Find("Button").GetComponent<UISprite>();
		l_description = transform.Find("Description").GetComponent<UILabel>();
		l_button = transform.Find("L_Button").GetComponent<UILabel>();
		C_Chest chest = prefab.GetComponent<C_Chest>();
		l_description.text = "'" + chest.eDescription + "'";
		if (locked) {
			s_button.spriteName = "ui_mobselect_locked";
			l_button.text = "[FFFFFF]Unlock at $level [00FE21]" + unlocksAt;
		} else {
			l_button.text = "Put down $gold" + chest.goldCost;
		}
	}
	
	public void unlock() {
		s_button.spriteName = "ui_mobselect_buy";
		l_button.text = "Put down $gold" + prefab.GetComponent<C_Chest>().goldCost;
		locked = false;
	}
}
