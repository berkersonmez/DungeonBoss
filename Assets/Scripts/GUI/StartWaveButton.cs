using UnityEngine;
using System.Collections;

public class StartWaveButton : MonoBehaviour {

	void OnClick() {
		GameController.instance.startNextDefenseImmediately();
	}
}
