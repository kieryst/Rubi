using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseController : MonoBehaviour {
	
	public static PauseController pause;

	public RectTransform pausedPopup;
	public Image image;
	public RectTransform quitButton;
	public Image quitImage;
	public Button quitBtn;
	public string pauseText;

	void Awake () {
		if (pause == null) {
			DontDestroyOnLoad (gameObject);
			pause = this;
		} else if (pause != this) {
			Destroy(gameObject);
		}
	}

	void Start () {
		image = pausedPopup.GetComponent<Image> ();
		image.color = new Color (1f, 1f, 1f, 0f);
		quitImage = quitButton.GetComponent<Image> ();
		quitImage.color = new Color (1f, 1f, 1f, 0f);
		quitBtn = quitButton.GetComponent<Button> ();
	}

	public void Pause () {
		PopupController.pop.DisplayPopup(pauseText);
		GameCamera.gameCamera.pseudoPause = true;
		image.color = new Color (1f, 1f, 1f, 1f);
		quitImage.color = new Color (1f, 1f, 1f, 1f);
		quitBtn.interactable = true;
		quitBtn.Select ();
	}
	public void Unpause () {
		PopupController.pop.HidePopup();
		GameCamera.gameCamera.pseudoPause = false;
		image.color = new Color (1f, 1f, 1f, 0f);
		quitImage.color = new Color (1f, 1f, 1f, 0f);
		quitBtn.interactable = false;
	}
}
