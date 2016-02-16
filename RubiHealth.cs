using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RubiHealth : MonoBehaviour {

	public static RubiHealth rubiHealth;

	private GameObject player;
	
	public float invulnTime;
	public bool currInvuln;
	public float hurtTime;
	public float xStagger;
	private float xTempStagger;
	public float yStagger;
	public Slider healthSlider;

	RubiControllerScript playerMovement; 
	SpriteRenderer playerSprite;
	Animator anim;
	public bool isDead;

	void Awake () {
		if (rubiHealth == null) {
			DontDestroyOnLoad (gameObject);
			rubiHealth = this;
		} else if (rubiHealth != this) {
			Destroy(gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		currInvuln = false;
		playerMovement = RubiControllerScript.rubiControl.GetComponent <RubiControllerScript> ();
		playerSprite = RubiControllerScript.rubiControl.GetComponent <SpriteRenderer> ();
		anim = RubiControllerScript.rubiControl.GetComponent <Animator> ();
		playerMovement.enabled = true;
	}

	public void TakeDamage (int amount)
	{
		// Set the damaged flag so the screen will flash.
		if (!currInvuln && !isDead) 
		{
			if (playerMovement.facingRight == true) {
			    xTempStagger = xStagger * -1;
			} else {
				xTempStagger = xStagger;
			}

			playerMovement.enabled = false;
			GetComponent<Rigidbody2D>().velocity = new Vector2(xTempStagger , yStagger);
			anim.SetBool ("Hurt", true);
			currInvuln = true;
			StartCoroutine (InvulnFlicker (invulnTime));
			StartCoroutine (HurtDisable (hurtTime));

			if (amount > GameVars.vars.currentHealth) {
				GameVars.vars.currentHealth = 0;
			} else {
				GameVars.vars.currentHealth -= amount;
			}

			// If the player has lost all it's health and the death flag hasn't been set yet...
			if (GameVars.vars.currentHealth <= 0 && !isDead) {
				Death ();
			}
		}
	}
	
	void Death ()
	{
		isDead = true;
        // anim.SetTrigger ("Die");
		PauseController.pause.pauseText = "Game Over";
		PauseController.pause.Pause ();
	}    

	private IEnumerator HurtDisable(float hurtTime) {
		yield return new WaitForSeconds (hurtTime);
		playerMovement.enabled = true;
		anim.SetBool ("Hurt", false);
	}

	private IEnumerator InvulnFlicker(float invulnTime) {
		float alpha = 1f;
		for (float t = 0f; t < invulnTime; t += Time.deltaTime) {
			if (alpha == 1f) {
				alpha = 0f;
			} else {
				alpha = 1f;
			}
			playerSprite.color = new Color(1f, 1f, 1f, alpha);
			yield return new WaitForSeconds(0.05f);
		}
		currInvuln = false;
		playerSprite.color = new Color(1f, 1f, 1f, 1f);
	}
}
