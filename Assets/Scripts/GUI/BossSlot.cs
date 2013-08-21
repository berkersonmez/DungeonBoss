using UnityEngine;
using System.Collections;

public class BossSlot : MonoBehaviour {

	public GameObject prefab;
	
	private UISprite s_avatar;
	private UILabel l_description;
	private UISprite s_button;
	private UILabel l_button;
	
	void Start() {
		s_avatar = transform.Find("Avatar").GetComponent<UISprite>();
		s_button = transform.Find("Button").GetComponent<UISprite>();
		l_description = transform.Find("Description").GetComponent<UILabel>();
		l_button = transform.Find("L_Button").GetComponent<UILabel>();
		
		M_Boss boss = prefab.GetComponent<M_Boss>();
		l_description.text = "'" + boss.eDescription + "'";
	}
}
