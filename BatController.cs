using UnityEngine;
using System.Collections;

public class BatController : MonoBehaviour {
	
	private AlphaParticleController part;
	public float maxSpeed = 2f;
	private bool tracking = false;
	public bool haveTracked;
	public float trackingDistance;
	public float despawnDistance;
	public float lostDistance;
	private bool facingRight = true;
	private bool dying = false;
	
	private Animator anim;
	
	public int bat_health = 2;
	public int bat_damage = 1;

	void Start () {
		anim = GetComponent<Animator> ();
	}

	void FixedUpdate () {
		if (tracking && !dying && !GameCamera.gameCamera.pseudoPause) {
			// Follow Rubi
			// To-Do: Raycasting to check for line of sight before following?
			GetComponent<Rigidbody2D>().velocity = (RubiControllerScript.rubiControl.gameObject.transform.position - transform.position).normalized * maxSpeed;
		} else {
			//Stop
			GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
		}
	}

	void OnTriggerEnter2D (Collider2D collider) {
		if (collider.gameObject.tag == "Player") {
			RubiHealth.rubiHealth.TakeDamage(bat_damage);
		}
		if (collider.gameObject.tag == "AlphaParticle") {
			part = collider.gameObject.GetComponent <AlphaParticleController> ();
			TakeDamage(part.damage);
			part.hit = true;
		}
	}
	void OnTriggerStay2D (Collider2D collider) {
		if (collider.gameObject.tag == "Player") {
			RubiHealth.rubiHealth.TakeDamage (bat_damage);
		}
	}

	void Update () {

		float distance = Vector3.Distance (RubiControllerScript.rubiControl.gameObject.transform.position, transform.position);
		if (distance <= trackingDistance) {
			// If Rubi is close enough, begin to follow. Set haveTracked flag for check to destroy gameObject if Rubi gets too far away.
			tracking = true;
			haveTracked = true;
		}
		if (distance > lostDistance) {
			tracking = false;
		}
		if (bat_health <= 0 && dying != true) {
			dying = true;
			// DestroyBat() will be called via animation event tied to "Die" trigger when it's finished playing.
			anim.SetTrigger ("Die");
		}

		//Flip bat sprite if Rubi is on the other side
		if ((RubiControllerScript.rubiControl.gameObject.transform.position.x - transform.position.x) < 0f && facingRight) {
			Vector3 theScale = GetComponent<Rigidbody2D> ().transform.localScale;
			theScale.x *= -1;
			GetComponent<Rigidbody2D> ().transform.localScale = theScale;
			facingRight = false;
		} else if ((RubiControllerScript.rubiControl.gameObject.transform.position.x - transform.position.x) > 0f && !facingRight) {
			Vector3 theScale = GetComponent<Rigidbody2D> ().transform.localScale;
			theScale.x *= -1;
			GetComponent<Rigidbody2D> ().transform.localScale = theScale;
			facingRight = true;
		}

		// If Rubi is far away, and the bat is not at its spawn, destroy.
		if (haveTracked && (distance >= despawnDistance)) {
			Destroy (gameObject);
		}
	}

	void TakeDamage (int damage) {
		bat_health -= damage;
	}

	void DestroyBat () {
		Destroy (gameObject);
	}
}
