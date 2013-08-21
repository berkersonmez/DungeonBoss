using UnityEngine;
using System.Collections;

public class ValueTable : MonoBehaviour {

	void OnClick() {
		string tooltipMessage;
		tooltipMessage = "$gold " + GameController.instance.gold;
		tooltipMessage += " $grudge " + GameController.instance.grudge;
		tooltipMessage += " $level " + GameController.instance.level;
		tooltipMessage += " $unit " + GameController.instance.units + "/" + GameController.instance.maxUnits;
		tooltipMessage += "\n-------------------------------\n";
		tooltipMessage += "$tp " + GameController.instance.talentPoints;
		tooltipMessage += " $exp " + GameController.instance.experience + "/" + GameController.instance.expRequired;
		UITooltip.ShowText(tooltipMessage);
	}
}
