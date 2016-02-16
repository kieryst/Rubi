using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PopupController : MonoBehaviour {

	public static PopupController pop;

	public RectTransform popup;
	public RectTransform rubiPortrait;
	public RectTransform rubiHmmPortrait;
	public RectTransform mithPortrait;
	public RectTransform popupArrow;
	public Text popupText;
	public Text dialogText;
	public Image image;
	public Image portrait;
	public Image rubi_portrait;
	public Image rubi_hmm_portrait;
	public Image mith_portrait;
	public Image arrow;
	public Text textformatting;
	public Text dialog;
	public bool dialogDisplay = false;

	public string dd;
	public string pp;

	public bool cycle = false;

	void Awake () {
		if (pop == null) {
			DontDestroyOnLoad (gameObject);
			pop = this;
		} else if (pop != this) {
			Destroy(gameObject);
		}
	}
	
	void Start () {
		image = popup.GetComponent<Image> ();
		textformatting = popupText.GetComponent<Text> ();
		rubi_portrait = rubiPortrait.GetComponent<Image> ();
		rubi_hmm_portrait = rubiHmmPortrait.GetComponent<Image> ();
		mith_portrait = mithPortrait.GetComponent<Image> ();
		dialog = dialogText.GetComponent<Text> ();
		arrow = popupArrow.GetComponent<Image> ();
		image.color = new Color (1f, 1f, 1f, 0f);
		textformatting.color = new Color (1f, 1f, 1f, 0f);
		dialog.color = new Color (1f, 1f, 1f, 0f);
		rubi_portrait.color = new Color (1f, 1f, 1f, 0f);
		rubi_hmm_portrait.color = new Color (1f, 1f, 1f, 0f);
		mith_portrait.color = new Color (1f, 1f, 1f, 0f);
		arrow.color = new Color (1f, 1f, 1f, 0f);
	}

	public void DisplayPopup (string str) {
		dialogDisplay = false;
		textformatting.text = str;
		StartCoroutine (FadeTo (false, 0.11f, 1f));
	}
	public void HidePopup () {
		StartCoroutine (FadeTo (false, 0.11f, 0f));
	}
	public void DisplayDialog (string d, string p, bool c) {
		dialogDisplay = true;

		dd = d;
		pp = p;

		StartCoroutine (FadeTo (c, 0.11f, 1f));
	}
	public void HideDialog () {
		StartCoroutine (FadeTo (false, 0.11f, 0f));
	}
	private IEnumerator FadeTo(bool cycle, float aTime, float aValue)
	{
		if (cycle) {
			for (float t = 0.0f; t <= 1.0f; t += Time.deltaTime / aTime)
			{
				Color newColor = new Color(1, 1, 1, Mathf.Lerp(1f,0f,t));
				image.color = newColor;
				dialog.color = newColor;
				portrait.color = newColor;
				arrow.color = newColor;
				yield return null;
			}
			image.color = new Color(1, 1, 1, 0);
			dialog.color = new Color (1, 1, 1, 0);
			portrait.color = new Color(1, 1, 1, 0);
			arrow.color = new Color(1, 1, 1, 0);
			cycle = false;
		}
		if (!cycle) {
			if (dialogDisplay) {
				dialog.text = dd;			
				if (pp == "rubi_portrait") {
					portrait = rubi_portrait;
				} else if (pp == "rubi_hmm_portrait") {
					portrait = rubi_hmm_portrait;
				} else if (pp == "mith_portrait") {
					portrait = mith_portrait;
				}
			}
			float alpha = image.color.a;
			for (float t = 0.0f; t <= 1.0f; t += Time.deltaTime / aTime) {
				Color newColor = new Color (1, 1, 1, Mathf.Lerp (alpha, aValue, t));
				image.color = newColor;
				if (dialogDisplay) {
					dialog.color = newColor;
					portrait.color = newColor;
					arrow.color = newColor;
				} else {
					textformatting.color = newColor;
				}
				yield return null;
			}
			image.color = new Color (1, 1, 1, aValue);
			if (dialogDisplay) {
				dialog.color = new Color (1, 1, 1, aValue);
				portrait.color = new Color (1, 1, 1, aValue);
				arrow.color = new Color (1, 1, 1, aValue);
			} else {
				textformatting.color = new Color (1, 1, 1, aValue);
			}
		}
	}
}
