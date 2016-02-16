using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class QuitButton : MonoBehaviour {

	public Object arrow;
	public GameObject newArrow;
	public Canvas canvas;

	public Button qutButton;

	public bool reallyQuit = false;
	
	void Start () {
		qutButton = GetComponent<Button> ();
		qutButton.interactable = false;
	}

	void Update () {
		if (qutButton.interactable == false && newArrow != null) {
			reallyQuit = false;
			Destroy (newArrow);
		}
		if (qutButton.interactable == true && newArrow == null) {
			newArrow = Instantiate (arrow, new Vector3 (GetComponent<RectTransform> ().position.x - 90, GetComponent<RectTransform> ().position.y, GetComponent<RectTransform> ().position.z), Quaternion.identity) as GameObject;
			newArrow.transform.SetParent (canvas.transform);
		}
	}

	public void Quit() {
		if (reallyQuit == false) {
			reallyQuit = true;
			PopupController.pop.DisplayPopup("Quit to Title Screen: A re you sure?");
		} else {
			RubiControllerScript.rubiControl.DestroyThis();
			if (MithController.mithControl != null) {
				MithController.mithControl.DestroyThis();
			}
			PauseController.pause.Unpause();
			Application.LoadLevel ("Start");
		}
	}
}
