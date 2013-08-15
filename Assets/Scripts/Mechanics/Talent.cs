using UnityEngine;
using System.Collections;

public class Talent {

	public string name;
	public int slotIndex;
	public bool active;
	public string description;
	public string spriteName;
	
	public Talent(int slotIndex, string name, string description, string spriteName) {
		this.slotIndex = slotIndex;
		this.name = name;
		this.description = description;
		this.spriteName = spriteName;
	}
}
