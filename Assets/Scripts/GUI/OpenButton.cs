using UnityEngine;
using System.Collections;

public class OpenButton : MonoBehaviour {

	public GameObject windowPanel;
	
	void OnClick() {
		NGUITools.SetActive(windowPanel, true);
	}
}
