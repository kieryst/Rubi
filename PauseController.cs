using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseController : MonoBehaviour {

	// Pause will set the game time scale to 0. Will also enable pause menu canvas elements.

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
		PopupController.pop.popupText.text = pauseText;
		PopupController.pop.DisplayPopup();
		Time.timeScale = 0;
		GameCamera.gameCamera.enabled = false;
		image.color = new Color (1f, 1f, 1f, 1f);
		quitImage.color = new Color (1f, 1f, 1f, 1f);
		quitBtn.interactable = true;
		quitBtn.Select ();
	}
	public void Unpause () {
		PopupController.pop.HidePopup();
		Time.timeScale = 1;
		GameCamera.gameCamera.enabled = true;
		image.color = new Color (1f, 1f, 1f, 0f);
		quitImage.color = new Color (1f, 1f, 1f, 0f);
		quitBtn.interactable = false;
	}
}
