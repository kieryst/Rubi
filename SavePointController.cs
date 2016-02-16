using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SavePointController : MonoBehaviour {

	public bool savable = false;
	public float locationX;
	public float locationY;

	void OnTriggerEnter2D (Collider2D collider) {
		if (collider.gameObject.tag == "Player") {
			GameVars.vars.currentX = locationX;
			GameVars.vars.currentY = locationY;
			savable = true;
			PopupController.pop.DisplayPopup("Press A  to save");
		}
	}
	void OnTriggerExit2D (Collider2D collider) {
		if (collider.gameObject.tag == "Player") {
			savable = false;
			PopupController.pop.HidePopup();
		}
	}
}
