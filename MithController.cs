using UnityEngine;
using System.Collections;

public class MithController : MonoBehaviour {

	// ToDo: set preference variables to ensure each of Mith's states gets called within a reasonable time frame.
	public static MithController mithControl;

	private Animator anim;

	private bool facingRight = true;

	public float speed = 600f;
	public int mithHealth = 12;
	private float currInvulnTime;
	public float invulnTime = 0.5f;
	public float flashTime = 0.1f;
	private float currFlashTime;
	bool flashing;

	private float dist_x;
	private float close_x;
	private float close_y;

	private float pref_x;
	private float neg_pref_x;
	private float pref_y;
	private float realPrefX;

	private int x_Vel;
	private int y_Vel;
	public float xOffset = 5f;
	public float yOffset = 5f;
	
	private float xFinalNorm;
	private float yFinalNorm;

	public bool bossEnabled = false;

	public bool moving = false;
	public bool charging = false;
	public bool phasing = false;
	public bool phased = false;

	public float timeMoving = 3f;
	public float fireDelay = 0.25f;
	public int fire;
	private int fireCounter;
	public float phaseDelay = 0.5f;
	public float phaseAnim = 0.5f;
	public float currTime;

	public GameObject muon_collapse;

	public bool dying = false;
	private bool currInvuln;
	private AlphaParticleController part;

	public float random;
	public float phaseX;
	public float phaseY;
	public Vector2 mithPhase1;
	public Vector2 mithPhase2;
	public Vector2 mithPhase3;
	public Vector2 mithPhase4;

	public SpriteRenderer render;
	BoxCollider2D[] colliders;

	//Singleton mode. Mith will recur on subsequent levels.
	void Awake () {
		if (mithControl == null) {
			DontDestroyOnLoad (gameObject);
			mithControl = this;
		} else if (mithControl != this) {
			Destroy(gameObject);
		}
	}

	// Set render because we adjust alpha throughout the fight. Set colliders because we enable/disable them throughout the fight.
	void Start () {
		anim = GetComponent<Animator> ();
		render = GetComponent<SpriteRenderer> ();
		colliders = gameObject.GetComponentsInChildren<BoxCollider2D> ();
	}

	void FixedUpdate() {

		// Mith took damage. Toggle invulnerable 'flashing' state for a short while. -or-
		// Mith is currently phasing to a different point in the fight. Set alpha accordingly.
		if (currInvuln) {
			currInvulnTime -= Time.deltaTime;
			if (currInvulnTime <= 0f) {
				currInvuln = false;
			}
			if (!phasing && !phased && !flashing && currFlashTime > 0) {
				render.color = new Color(1f, 1f, 1f, 0f);
				currFlashTime -= Time.deltaTime;
			} else if (!phasing && !phased && !flashing && currFlashTime <= 0) {
				flashing = true;
				currFlashTime = flashTime;
			} else if (!phasing && !phased && flashing && currFlashTime > 0) {
				render.color = new Color(1f, 1f, 1f, 1f);
				currFlashTime -= Time.deltaTime;
			} else if (!phasing && !phased && flashing && currFlashTime <= 0) {
				flashing = false;
				currFlashTime = flashTime;
			}
		} else if (!phasing && !phased) {
			render.color = new Color(1f, 1f, 1f, 1f);
			flashing = false;
		}

		if (mithHealth <= 0 && dying != true) {
			dying = true;
			StartCoroutine (Die());
		}

		// Boss fight is underway. Keep track of 'desirable' coords to hover Mith at (Diagonal and up to Rubi's current position.)
		if (bossEnabled) {

			dist_x = RubiControllerScript.rubiControl.gameObject.transform.position.x - transform.position.x;;

			// Flip Sprite to face current direction
			if (dist_x > 0 && !facingRight)
				Flip ();
			else if (dist_x < 0 && facingRight)
				Flip ();

			// Moving state
			if (moving) {

				pref_x  = RubiControllerScript.rubiControl.gameObject.transform.position.x + xOffset;
				neg_pref_x = RubiControllerScript.rubiControl.gameObject.transform.position.x - xOffset;
				pref_y = RubiControllerScript.rubiControl.gameObject.transform.position.y + yOffset;

				// Keep track of desirable X coords to Left and Right of Rubi. (If Mith gets pinched against a wall, she will switch to the opposite X coord.)
				if (Mathf.Abs (transform.position.x - pref_x ) < Mathf.Abs (transform.position.x - neg_pref_x)) {
					realPrefX = pref_x;
				} else {
					realPrefX = neg_pref_x;
				}

				if (transform.position.x > realPrefX) {
					x_Vel = -1;
				} else {
					x_Vel = 1;
				}

				if (transform.position.y > pref_y) {
					y_Vel = -1;
				} else {
					y_Vel = 1;
				}

				// Mith's movements will behave 'magnetically'. She will travel with greater 'Force' to her desired X location if Rubi is very close or very far to her.
				close_x = Mathf.Abs(transform.position.x - realPrefX);
				close_y = Mathf.Abs (transform.position.y - pref_y);

				xFinalNorm = close_x / xOffset;
				yFinalNorm = close_y / yOffset;

				GetComponent<Rigidbody2D>().AddForce(new Vector2(speed * x_Vel * xFinalNorm, speed * y_Vel * yFinalNorm));

				// Moving phase is over. Transition to either teleport or attack.
				if (currTime > 0) {
					currTime -= Time.deltaTime;
				} else {
					moving = false;
					random = Random.Range (0f,1f);
					if (random < 0.5f) {
						fireCounter = fire;
						Charging ();
					} else {
						StartPhasing ();
					}
				}
			}

			// Attacking state
			if (charging) {
				//Mith will fire a number of 'Muon Collapse' projectiles at Rubi in succession.
				if (fireCounter > 0 && currTime < 0) {
					GameObject shot = Instantiate (muon_collapse, GetComponent<Rigidbody2D>().position, Quaternion.identity) as GameObject;
					Vector3 theScale = shot.transform.localScale;
					// Projectiles are larger than standard Muon Collapse ones.
					theScale.x = 2;
					theScale.y = 2;
					shot.transform.localScale = theScale;
					fireCounter = fireCounter -1 ;
					currTime = fireDelay;
				} else if (fireCounter > 0) {
					currTime -= Time.deltaTime;
				} else {
					// Attacking sequence is over. Transition into Moving or Phasing states.
					anim.SetBool ("Attacking", false);
					charging = false;
					fireCounter = fire;
					random = Random.Range (0f,1f);
					if (random < 0.5f) {
						StartMoving ();
					} else {
						StartPhasing ();
					}
				}
			}

			// Teleporting state. Play a short 'channeling' animation before moving Mith.
			if (phasing) {
				if (currTime > 0) {
					currTime -= Time.deltaTime;
				} else {
					phasing = false;
					currTime = phaseDelay;
					phased = true;
					anim.SetBool("Phasing", false);
				}
			}

			// Teleport Mith.
			if (phased) {
				if (currTime > 0) {
					currTime -= Time.deltaTime;
				} else {
					transform.position = new Vector2(phaseX, phaseY);
					phased = false;
					random = Random.Range (0f,1f);
					//Re-enable alpha and hitbox colliders
					render.color = new Color (1f, 1f, 1f, 1f);
					foreach (BoxCollider2D bc in colliders) {
						bc.enabled = true;
					}
					// Teleporting over. transition into moving or attacking states.
					if (random < 0.5f) {
						StartMoving();
					} else {
						Charging();
					}
				}
			}
		}
	}

	void OnTriggerEnter2D (Collider2D collider) {
		if (collider.gameObject.tag == "Player") {
			if (!dying) {
				RubiHealth.rubiHealth.TakeDamage(1);
			}
		}
		if (collider.gameObject.tag == "AlphaParticle") {
			part = collider.gameObject.GetComponent <AlphaParticleController> ();
			TakeDamage(part.damage);
			part.hit = true;
		}
	}

	void TakeDamage (int damage) {
		
		// Set the damaged flag so the screen will flash.
		if (!currInvuln) 
		{
			currInvuln = true;
			currInvulnTime = invulnTime;
			
			if (damage >= mithHealth ) {
				mithHealth = 0;
			} else {
				mithHealth -= damage;
				//anim.SetBool ("Hurt", true);
			}
		}
	}

	//Teleporting. Pick one of 4 points on the fight stage that Mith can teleport to and set coords.
	void StartPhasing () {
		anim.SetBool ("Phasing", true);
		random = Random.Range (0f, 1f);
		if (random < 0.25f) {
			phaseX = mithPhase1.x;
			phaseY = mithPhase1.y;
		} else if (random < 0.5f) {
			phaseX = mithPhase2.x;
			phaseY = mithPhase2.y;
		} else if (random < 0.75f) {
			phaseX = mithPhase3.x;
			phaseY = mithPhase3.y;
		} else {
			phaseX = mithPhase4.x;
			phaseY = mithPhase4.y;
		}
		currTime = phaseAnim;
		phasing = true;
	}

	void Dissapear () {
		render.color = new Color (1f, 1f, 1f, 0f);
		foreach (BoxCollider2D bc in colliders) {
			bc.enabled = false;
		}
	}

	void Charging () {
		anim.SetBool ("Attacking", true);
		currTime = fireDelay;
		charging = true;
	}

	public void StartMoving () {
		currTime = timeMoving;
		moving = true;
	}

	void Flip()
	{
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

    // ToDo: Re-work Mith first boss ending sequence. Needs dialog rather than just destroying the object
	private IEnumerator Die() {
		GameVars.vars.boss1 = true;
		Vector3 theScale = GetComponent<Rigidbody2D>().transform.localScale;
		if (!facingRight) {
			theScale.x *= -1;
			GetComponent<Rigidbody2D>().transform.localScale = theScale;
		}
		anim.SetTrigger("Die");
		yield return new WaitForSeconds(0.084f);
		Destroy(gameObject);
	}
	public void DestroyThis()
	{
		Destroy(gameObject);
	}
}
