using UnityEngine;
using System.Collections;

public class StartGameButton : MonoBehaviour {

	void OnClick() {
		NGUITools.SetActive(MainMenuInput.instance.bossSelectWindow, true);
	}
}
