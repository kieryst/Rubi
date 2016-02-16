using UnityEngine;
using System.Collections;

public class StartButtonController : MonoBehaviour {
	
	public GameObject redRubi;

	public Object arrow;
	public GameObject newArrow;
	public Canvas canvas;

	void NewGamePressed () {
		Application.LoadLevel("Load");
	}

	public void onHighlight () {
		newArrow = Instantiate (arrow, new Vector3 (GetComponent<RectTransform> ().position.x - 90, GetComponent<RectTransform> ().position.y, GetComponent<RectTransform> ().position.z), Quaternion.identity) as GameObject;
		newArrow.transform.SetParent (canvas.transform);
	}
	public void onAway () {
		Destroy (newArrow);
	}
}

