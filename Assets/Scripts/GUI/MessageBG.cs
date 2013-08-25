using UnityEngine;
using System.Collections;

public class MessageBG : MonoBehaviour {

	void OnClick() {
		MessageBox.instance.skipToNextMessage();
	}
}
