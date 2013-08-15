using UnityEngine;
using System.Collections;

public class TalentSlot : MonoBehaviour {
	public Talent talent;
	
	private TalentWindow window;
	
	void Start() {
		window = transform.parent.GetComponent<TalentWindow>();
	}
	
	public void setTalent(Talent talent) {
		UISprite sprite = GetComponent<UISprite>();
		this.talent = talent;
		if (talent.active) {
			sprite.color = new Color(1f, 1f, 1f);
		} else {
			sprite.color = new Color(.5f, .5f, .5f);
		}
		sprite.spriteName = talent.spriteName;
	}
	
	void OnPress(bool isDown) {
		if (isDown) {
			Invoke("onHoldDown", .5f);
		} else {
			if (IsInvoking("onHoldDown")) {
				CancelInvoke("onHoldDown");
				onClick();
			}
		}
	}
	
	void onHoldDown() {
		string tooltip = "[2FC9DE]" + talent.name + "[-]\n";
		if (!talent.active && !window.checkRequisite(talent.slotIndex)) {
			tooltip += "[FF0000]Requires prerequisite talent to be activated.[-]\n";
		} else if (!talent.active && GameController.instance.talentPoints == 0) {
			tooltip += "[FF0000]Not enough $tp.[-]\n";
		} else if (!talent.active) {
			tooltip += "[6BC963]Activate talent by paying $tp 1.[-]\n";
		}
		tooltip += talent.description;
		UITooltip.ShowText(tooltip);
	}
	
	void activate() {
		UISprite sprite = GetComponent<UISprite>();
		sprite.color = new Color(1f, 1f, 1f);
		talent.active = true;
	}
	
	void onClick() {
		if (!talent.active && window.checkRequisite(talent.slotIndex) && GameController.instance.talentPoints > 0) {
			GameController.instance.talentPoints -= 1;
			activate();
		}
	}
}
