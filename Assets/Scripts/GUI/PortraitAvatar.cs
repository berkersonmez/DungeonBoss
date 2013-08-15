using UnityEngine;
using System.Collections;

public class PortraitAvatar : MonoBehaviour {

	void OnClick() {
		transform.parent.GetComponent<Portrait>().character.showTooltip();
	}
}
