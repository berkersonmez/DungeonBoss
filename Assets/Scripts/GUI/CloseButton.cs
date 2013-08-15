using UnityEngine;
using System.Collections;

public class CloseButton : MonoBehaviour {

	public GameObject windowPanel;
	
	void OnClick() {
		NGUITools.SetActive(windowPanel, false);
	}
}
