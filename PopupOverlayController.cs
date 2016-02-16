using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PopupOverlayController : MonoBehaviour {

	public static PopupOverlayController overlay;
	public Image image;
	public Image popup;
	public float duration;

	void Awake () {
		if (overlay == null) {
			DontDestroyOnLoad (gameObject);
			overlay = this;
		} else if (overlay != this) {
			Destroy(gameObject);
		}
	}
	
	void Start () {
		image = overlay.GetComponent<Image> ();
		popup = PopupController.pop.GetComponent<Image> ();
		image.color = new Color (1f, 1f, 1f, 0f);
	}

	public void PushFlash () {
		StartCoroutine (Flash());
	}

	// Adjusts the flash overlay and size of the text box, as a visual indicator that a button was pressed.
	// Used when games are saved.
	// Strength of overlay alpha and text box size are derived off of the function -(x-1)^2+1

	private IEnumerator Flash()	{
		for (float t = 0.0f; t <= 1.0f; t += Time.deltaTime / duration) {
			image.color = new Color(1f, 1f, 1f, -(Mathf.Pow(2,t-1))+1);
			image.transform.localScale = new Vector3(1 - (-(Mathf.Pow(2,t-1))+1)*0.1f, 1 - (-(Mathf.Pow(2,t-1))+1)*0.1f, 1);
			popup.transform.localScale = new Vector3(1 - (-(Mathf.Pow(2,t-1))+1)*0.1f, 1 - (-(Mathf.Pow(2,t-1))+1)*0.1f, 1);
			yield return null;			
		}
		image.color = new Color (1f, 1f, 1f, 0f);
		image.transform.localScale = new Vector3(1, 1, 1);
		popup.transform.localScale = new Vector3(1, 1, 1);
	}
}