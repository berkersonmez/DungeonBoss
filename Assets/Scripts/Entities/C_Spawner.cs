using UnityEngine;
using System.Collections;

public class C_Spawner : MonoBehaviour {

	public GameObject portrait;
	
	public GameObject[] portraits;
	
	public int charsLeft = -1;
	
	public void signalCharDied() {
		charsLeft--;
		GameController.instance.charDied();
		if (charsLeft == 0) {
			foreach( GameObject prt in portraits ) {
				Destroy(prt);
			} 
			GameController.instance.endRound();
		}
	}
	
	public void spawn(int charID) {
		int numberOfChars = 1;
		portraits = new GameObject[numberOfChars];
		for (int i = 0 ; i < numberOfChars ; i++) {
			int prefabIndex = GameController.instance.idToPrefab[charID];
			GameObject chr = Instantiate(GameController.instance.charPrefabs[prefabIndex], transform.position, transform.rotation) as GameObject;
			GameObject prt = Instantiate(portrait, portrait.transform.position, portrait.transform.rotation) as GameObject;
			prt.transform.parent = GameObject.Find("Left Portraits").transform;
			prt.transform.localScale = new Vector3(1f, 1f, 1f);
			prt.transform.localPosition = new Vector3(0f, 0f - 400f * i , -1090.77f);
			prt.transform.localRotation = Quaternion.identity;
			
			portraits[i] = prt;
			prt.GetComponent<Portrait>().setChar(chr.GetComponent<M_Char>());
		}
		charsLeft = numberOfChars;
	}
	
	public void spawn() {
		int spawnCount = calculateSpawnCount();
		portraits = new GameObject[spawnCount];
		for (int i = 0 ; i < spawnCount ; i++) {
			GameObject chr = Instantiate(getRandomCharPrefab(), transform.position, transform.rotation) as GameObject;
			GameObject prt = Instantiate(portrait, portrait.transform.position, portrait.transform.rotation) as GameObject;
			prt.transform.parent = GameObject.Find("Left Portraits").transform;
			prt.transform.localScale = new Vector3(1f, 1f, 1f);
			prt.transform.localPosition = new Vector3(0f, 0f - 400f * i , -1090.77f);
			prt.transform.localRotation = Quaternion.identity;
			
			portraits[i] = prt;
			prt.GetComponent<Portrait>().setChar(chr.GetComponent<M_Char>());
		}
		charsLeft = spawnCount;
	}
	
	private GameObject getRandomCharPrefab() {
		int randomPrefabID = Random.Range(0, GameController.instance.charPrefabs.Length);
		return GameController.instance.charPrefabs[randomPrefabID];
	}
	
	private int calculateSpawnCount() {
		int n = Mathf.Clamp(GameController.instance.round, 0, 39);
		int m = GameController.instance.round % 10;
		n = n/10 + 1;
		int r = Random.Range(0, m);
		r = r / 2;
		r = Mathf.Clamp(r + GameController.instance.round / 30, 1, n);
		return r;
	}
	
	void Update () {
		
	}
}
