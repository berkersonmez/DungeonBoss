using UnityEngine;
using System.Collections;

public class SaveSlot : MonoBehaviour {

	private UILabel l_description;
	private SaveController.SaveGame saveGame;
	private int saveID;
	
	public void setSaveGame(SaveController.SaveGame sg, int id) {
		l_description = transform.Find("Description").GetComponent<UILabel>();
		saveGame = sg;
		saveID = id;
		l_description.text = "SAVE NO. " + id + "\n";
		l_description.text += "Round " + saveGame.round + "\n";
		l_description.text += "$level " + saveGame.level + "\n";
		l_description.text += "$gold " + saveGame.gold + "\n";
		l_description.text += "$grudge " + saveGame.grudge + "\n";
		l_description.text += saveGame.saveTime + "\n";
	}
	
	public void selectThisSlot() {
		SaveController.instance.setToLoad(saveGame, saveID);
	}
}
