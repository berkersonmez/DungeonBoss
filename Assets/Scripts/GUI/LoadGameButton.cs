using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadGameButton : MonoBehaviour {

	void OnClick() {
		NGUITools.SetActive(MainMenuInput.instance.saveSelectWindow, true);
		SaveController.instance.prepareSaveSelectWindow();
	}
}
