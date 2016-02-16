using UnityEngine;
using System.Collections;

public class MuonBehavior : MonoBehaviour {

	// ToDo: Link a spawn object with it's associated 'Muon'. That way if conditions for Muon despawn are met, we can instantiate a new Muon at the spawn location, instead of only checking for a collision at the spawn location.
	private GameObject player;
	private bool playerFound = false;
	private AlphaParticleController part;
	private RubiHealth rubiHealth;
	private bool facingRight = true;
	private Animator anim;
	private bool dying = false;
	private float rand;
	public GameObject muon_collapse;
	public GameObject health_pickup;

	public bool tracking;
	public bool haveTracked;
	public float maxTimeBetweenActions;
	public float minTimeBetweenActions;
	private bool waiting = false;
	private float currTime = 0f;
	private int switcher = 1;
	public float jumpForce;
	public float trackingDistance;
	public float despawnDistance;
	public float lostDistance;

	bool grounded = false;
	public Transform groundCheck;
	public float groundRadius;
	public LayerMask whatIsGround;
	public float skin;
	
	private Vector3 c;
	private CircleCollider2D collider;

	public bool currInvuln;
	public float invulnTime;
	SpriteRenderer muonSprite;
	
	public float healthDropChance;

	public int muon_health;

	private Vector3 tempVelo;

	void Start () {
		anim = GetComponent<Animator> ();
		collider = GetComponent<CircleCollider2D>();
		c = collider.offset;
		muonSprite = GetComponent <SpriteRenderer> ();
	}

	void Update () {
		if (!GameCamera.gameCamera.pseudoPause) {
			if (GetComponent<Rigidbody2D>().isKinematic == true) {
				GetComponent<Rigidbody2D>().isKinematic = false;
				GetComponent<Rigidbody2D>().velocity = tempVelo;
			}

			// Follow Rubi if she is close enough. Also set tracked flag to despawn Muon if Rubi goes too far away.
			float distance = Vector3.Distance (RubiControllerScript.rubiControl.gameObject.transform.position, transform.position);
			if (distance <= trackingDistance) {
				tracking = true;
				if (!haveTracked) {
					haveTracked = true;
				}
			}

			if (distance > lostDistance) {
				tracking = false;
			}

			if (muon_health <= 0 && dying != true) {
				dying = true;
				float random = Random.Range (0f, 1f);
				if (random < healthDropChance) {
					Instantiate (health_pickup, GetComponent<Rigidbody2D> ().position, Quaternion.identity);
				}
				anim.SetTrigger ("Die");
			}
			if (!waiting) {
				currTime = Random.Range (minTimeBetweenActions, maxTimeBetweenActions);
				waiting = true;
			}
			if (waiting && currTime > 0 && tracking) {
				currTime -= Time.deltaTime;
			} else if (waiting && currTime <= 0 && tracking) {
				waiting = false;

				if ((RubiControllerScript.rubiControl.gameObject.transform.position.x - transform.position.x) < 0f && facingRight) {
					Flip ();
					facingRight = false;
					switcher = -1;
				} else if ((RubiControllerScript.rubiControl.gameObject.transform.position.x - transform.position.x) > 0f && !facingRight) {
					Flip ();
					facingRight = true;
					switcher = 1;
				}
				if (grounded) {
					GetComponent<Rigidbody2D> ().AddForce (new Vector2 (jumpForce * 0.6f * switcher, jumpForce * 1.5f));
				}
			}

			if (haveTracked && (distance >= despawnDistance)) {
				Destroy (gameObject);
			}
		} else {
			if (!GetComponent<Rigidbody2D>().isKinematic) {
				tempVelo = GetComponent<Rigidbody2D>().velocity;
			}
			GetComponent<Rigidbody2D>().isKinematic = true;
		}
	}

	// Things are getting hit.
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
	void OnTriggerStay2D (Collider2D collider) {
		if (collider.gameObject.tag == "Player") {
			if (!dying) {
				RubiHealth.rubiHealth.TakeDamage(1);
			}
		}
	}

	void TakeDamage (int damage) {
		// Set the damaged flag so the screen will flash.
		if (!currInvuln) 
		{
			currInvuln = true;				
			if (damage >= muon_health ) {
				muon_health = 0;
			} else {
				muon_health -= damage;
				anim.SetBool ("Hurt", true);
				Instantiate (muon_collapse, GetComponent<Rigidbody2D>().position, Quaternion.identity);
				StartCoroutine(InvulnFlicker (invulnTime));
			}
		}
	}
	public void DestroyMuon() {
		Destroy (gameObject);
	}
	void Flip () {
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
	void FixedUpdate () {

		float dir = -1;
		Vector2 p = transform.position;

		float x = p.x + c.x;
		float y = p.y + c.y * dir;

		RaycastHit2D hit = Physics2D.Raycast(new Vector2(x, y), new Vector2(0, -1), 1f, 1 << LayerMask.NameToLayer ("Ground"));
		if (hit.collider!= null) {
			float distance = hit.fraction;
			if (distance < skin) { 
				grounded = true;
			} else {
				grounded = false;
			}
		}
		
		if (grounded) {
			GetComponent<Rigidbody2D>().velocity = new Vector2 (0f, GetComponent<Rigidbody2D>().velocity.y);
		}
	}

	private IEnumerator InvulnFlicker(float invulnTime) {
		float alpha = 1f;
		for (float t = 0f; t < invulnTime; t += Time.deltaTime) {
			if (alpha == 1f) {
				alpha = 0f;
			} else {
				alpha = 1f;
			}
			muonSprite.color = new Color(1f, 1f, 1f, alpha);
			yield return new WaitForSeconds(0.05f);
		}
		currInvuln = false;
		muonSprite.color = new Color(1f, 1f, 1f, 1f);
		anim.SetBool ("Hurt", false);
	}
}
