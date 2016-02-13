using UnityEngine;
using System.Collections;

public class BackButton : MonoBehaviour {

	public Object arrow;
	public GameObject newArrow;
	public Canvas canvas;
	public string backScene;

	public void BackScene () {
		Application.LoadLevel (backScene);
	}
	public void onHighlight () {
		newArrow = Instantiate (arrow, new Vector3 (GetComponent<RectTransform> ().position.x - 90, GetComponent<RectTransform> ().position.y, GetComponent<RectTransform> ().position.z), Quaternion.identity) as GameObject;
		newArrow.transform.SetParent (canvas.transform);
	}
	public void onAway () {
		Destroy (newArrow);
	}
}
