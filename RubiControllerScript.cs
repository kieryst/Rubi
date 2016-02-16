using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RubiControllerScript : MonoBehaviour {

	public static RubiControllerScript rubiControl;

	public GameObject player;
	public float maxSpeed;
	public bool facingRight = true;
	Animator anim;

	public bool grounded = false;
	public bool crouched = false;
	public Transform groundCheck;
	public LayerMask whatIsGround;
	public float jumpForce;
	private int holdJump = 4;
	private bool jumpFlag = true;
	public float maxFallSpeed;
	private int runStep = 0;
	public float stepExpireTime;

	RubiEnergy rubiEnergy;
	public GameObject alphaParticle;
	public int alphaParticleCost = 3;

	public LayerMask collisionMask;
	public float skin = .016f;
	private Vector3 s;
	private Vector3 c;
	private BoxCollider2D collider;
	private float pScale;

	private float leftHit;
	private float rightHit;
	
	private float zeroHit;
	private float oneHit;
	private float twoHit;

	public Text popupText;

	public bool saveActive;
	public bool switchActive;
	private SwitchController currSwitch;

	public bool paused = false;

	void Awake () {
		if (rubiControl == null) {
			DontDestroyOnLoad (gameObject);
			rubiControl = this;
		} else if (rubiControl != this) {
			Destroy(gameObject);
		}
	}
	
	void Start () {
		DontDestroyOnLoad (gameObject);

		anim = player.GetComponent<Animator> ();
		StartCoroutine(SetStep ());

		rubiEnergy = player.GetComponent <RubiEnergy> ();
		collider = GetComponent<BoxCollider2D>();
		s = collider.size;
		c = collider.offset;
	}


	void OnTriggerEnter2D (Collider2D collider) {
		if (collider.gameObject.tag == "SavePoint") {
			saveActive = true;
		}
		if (collider.gameObject.tag == "Switch") {
			switchActive = true;
			currSwitch = collider.gameObject.GetComponent<SwitchController>();
		}
	}
	void OnTriggerExit2D (Collider2D collider) {
		if (collider.gameObject.tag == "SavePoint") {
			saveActive = false;
		}
		if (collider.gameObject.tag == "Switch") {
			switchActive = false;
			currSwitch = null;
		}
	}	    

	void Update()
	{

		if (!GameCamera.gameCamera.pseudoPause) {

			if (GetComponent<Rigidbody2D> ().isKinematic == true) {
				GetComponent<Rigidbody2D> ().isKinematic = false;
			}
			
			anim.SetFloat ("yTilt", Input.GetAxisRaw ("Vertical"));
			anim.SetBool("Ground", grounded);
			anim.SetFloat ("vSpeed", GetComponent<Rigidbody2D>().velocity.y);
			float move = Input.GetAxisRaw ("Horizontal");
			anim.SetFloat ("Speed", Mathf.Abs (move));
			
			// Flip Sprite to face current direction
			if (move > 0 && !facingRight)
				Flip ();
			else if (move < 0 && facingRight)
				Flip ();

			float min = 1;

			// Raycasting for Grounded check

			// ToDo, need a secondary frame collider to buffer against overlapping wierdness that Unity likes to do.
			for (int i = 0; i < 3; i ++) {

				Vector3 p = transform.position;
				pScale = transform.localScale.x;			
				float x = (p.x + (c.x*pScale) - s.x/2) + s.x/2 * i;			
				float y = p.y + c.y + s.y/2 * -1;
				
				RaycastHit2D hit = Physics2D.Raycast(new Vector2(x, y), -Vector2.up, 1f, 9 << LayerMask.NameToLayer("Ground"));

				if (i == 0) {
					RaycastHit2D sideHit = Physics2D.Raycast(new Vector2(x,y), new Vector2(-1, 0), 1f, 9 << LayerMask.NameToLayer ("Ground"));
					leftHit = sideHit.fraction;
					zeroHit = hit.fraction;
				} else if (i == 1) {
					oneHit = hit.fraction;
				} else if (i == 2) {
					RaycastHit2D sideHit = Physics2D.Raycast(new Vector2(x,y), new Vector2(1, 0), 1f, 9 << LayerMask.NameToLayer ("Ground"));
					rightHit = sideHit.fraction;
					twoHit = hit.fraction;
				}
				if (hit.fraction != 0) {
					float distance = hit.fraction;
					if (distance < min) {
						min = distance;
					}
				}
			}


			// Check to see if Grounded
			if (min < skin) {
				if ((zeroHit < skin || twoHit < skin) && (leftHit < skin || rightHit < skin) && oneHit > skin) {
					grounded = false;
				} else if (GetComponent<Rigidbody2D>().velocity.y > 0.1) {
					grounded = false;
				} else {
					grounded = true;
				}
			} else {;
				grounded = false;
			}

			GetComponent<Rigidbody2D> ().velocity = new Vector2 (move * maxSpeed, GetComponent<Rigidbody2D> ().velocity.y);
			
			// Inhibit falling speed
			if (GetComponent<Rigidbody2D> ().velocity.y < maxFallSpeed) {
				GetComponent<Rigidbody2D> ().velocity = new Vector2 (GetComponent<Rigidbody2D> ().velocity.x, maxFallSpeed);
			}

			// Single Jump
			if (grounded && Input.GetButtonDown ("Jump")) {
				anim.SetBool ("Ground", false);
				jumpFlag = true;
				GetComponent<Rigidbody2D> ().velocity = new Vector2 (GetComponent<Rigidbody2D> ().velocity.x, 0);
				GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0, jumpForce));
				StartCoroutine(HoldJump());
			}

			if (!Input.GetButton ("Jump") && jumpFlag) {
				jumpFlag = false;
				holdJump = 0;
			}

			if (grounded && (Input.GetAxisRaw ("Vertical") == -1) && (Input.GetAxisRaw ("Horizontal") == 0)) {
				crouched = true;
				anim.SetBool ("crouch", true);				
			} else {
				crouched = false;
				anim.SetBool ("crouch", false);
			}

			// Alpha Particle Attack
			if (Input.GetButtonDown ("Fire1") && !saveActive && !switchActive) {
				if (GameVars.vars.haveAlphaParticle && !crouched) {
					if (rubiEnergy.currentEnergy >= alphaParticleCost) {

						// ToDo: Re-work ground based attacks. Account for case where standing grounded attack sprite is played, and player moves before transition back to Idle
						if (grounded && Input.GetAxisRaw ("Horizontal") == 0 && Input.GetAxisRaw ("Vertical") == 0) {
							anim.SetTrigger ("groundAttack");
						} else if (grounded && Input.GetAxisRaw ("Horizontal") == 0 && Input.GetAxisRaw ("Vertical") == 1) {
							anim.SetTrigger ("groundUpAttack");

						} else if (grounded) {
							StartCoroutine (SetAnim ("Running Attack", 0.2f));
						} else if (Input.GetAxisRaw ("Vertical") == 0) {
							StartCoroutine (SetAnim ("jumpSideAttack", 0.2f));
						} else if (Input.GetAxisRaw ("Vertical") > 0) {
							StartCoroutine (SetAnim ("jumpUpAttack", 0.2f));
						} else if (Input.GetAxisRaw ("Vertical") < 0) {
							StartCoroutine (SetAnim ("jumpDownAttack", 0.2f));
						}
						rubiEnergy.SpendEnergy (alphaParticleCost);
						Instantiate (alphaParticle, GetComponent<Rigidbody2D> ().position, Quaternion.identity);
					}
				}
			} else if (Input.GetButtonDown ("Fire1") && saveActive) {
				GameVars.vars.Save ();
				PopupController.pop.popupText.text = "Game Saved!";
				PopupOverlayController.overlay.PushFlash ();
			} else if (Input.GetButtonDown ("Fire1") && switchActive) {
				currSwitch.powered = true;
				PopupOverlayController.overlay.PushFlash ();
			}
		} else {
			if (GetComponent<Rigidbody2D>().isKinematic == false) {
				GetComponent<Rigidbody2D>().isKinematic = true;
			}
		}

		if (!paused && Input.GetKeyDown (KeyCode.Escape)) {
			paused = true;
			PauseController.pause.pauseText = "Paused";
			PauseController.pause.Pause ();
		} else if (paused && Input.GetKeyDown (KeyCode.Escape) && !RubiHealth.rubiHealth.isDead) {
			paused = false;
			PauseController.pause.Unpause ();
		}
	}

	void Flip()
	{
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	private IEnumerator HoldJump () {
		while (jumpFlag && holdJump < 4) {
			yield return new WaitForSeconds (0.05f);
			if (Input.GetButton ("Jump")) {
				GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0, jumpForce / 8));
				holdJump+=1;
			}
		}
	}

	private IEnumerator SetStep() {
		while(true) {
			if (Input.GetAxisRaw ("Horizontal")==0) {
				runStep = 0;
			} else {
				runStep += 1;
				if (runStep >= 7) {
					runStep = 0;
				}
			}
			anim.SetFloat ("RunStep", runStep);
			yield return new WaitForSeconds (stepExpireTime);
		}
	}

	private IEnumerator SetAnim(string name, float duration) {
		anim.SetBool ("Running Attack", false);
		anim.SetBool ("jumpSideAttack", false);
		anim.SetBool ("jumpDownAttack", false);
		anim.SetBool ("jumpUpAttack", false);
		anim.SetBool (name, true);
		yield return new WaitForSeconds (duration);
		anim.SetBool (name, false);
	}

	public void DestroyThis()
	{
		Destroy(gameObject);
	}
}
