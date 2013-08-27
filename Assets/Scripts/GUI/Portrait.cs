using UnityEngine;
using System.Collections;

public class Portrait : MonoBehaviour {

	public M_Char character;
	
	private UISprite avatar;
	private UILabel l_health;
	private UILabel l_mana;
	private UILabel l_nickname;
	private UISlider s_health;
	private UISlider s_mana;
	
	public void setChar(M_Char character) {
		this.character = character;
		avatar = transform.Find("Avatar").GetComponent<UISprite>();
		l_health = transform.Find("Health").Find("H_Text").GetComponent<UILabel>();
		l_mana = transform.Find("Mana").Find("H_Mana").GetComponent<UILabel>();
		l_nickname = transform.Find("Nickname").GetComponent<UILabel>();
		s_health = transform.Find("Health").GetComponent<UISlider>();
		s_mana = transform.Find("Mana").GetComponent<UISlider>();
		
		l_nickname.text = character.nickname;
		avatar.spriteName = GameController.instance.portraitSprites[character.id];
	}
	
	void Update() {
		l_health.text = character.health + "/" + character.maxHealth;
		l_mana.text = character.mana + "/" + character.maxMana;
		s_health.sliderValue = (float) character.health / character.maxHealth;
		s_mana.sliderValue = (float) character.mana / character.maxMana;
	}
	
}
