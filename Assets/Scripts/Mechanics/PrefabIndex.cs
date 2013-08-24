using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PrefabIndex : MonoBehaviour {
	public static PrefabIndex instance;
	
	public GameObject[] prefabIndex = new GameObject[1];

	void Start () {
		instance = this;
	}
}
