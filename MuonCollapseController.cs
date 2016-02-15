using UnityEngine;
using System.Collections;

public class MuonCollapseController : MonoBehaviour {
	
	public float lifeSpan = 2f;
	private float currLife;
	private bool dying;
	private Vector2 velocity;
	public int damage;
	private RubiHealth rubiHealth;
	public float speed;
	
	void Start () {
		velocity.x = Random.Range (-3.0f, 3.0f);
		velocity.y = 4f;
		GetComponent<Rigidbody2D>().velocity = new Vector2 (velocity.x, velocity.y);
	}

	void OnTriggerEnter2D (Collider2D collider) {
		rubiHealth = RubiControllerScript.rubiControl.GetComponent <RubiHealth> ();
		if (collider.gameObject.tag == "Player") {
			if (!dying) {
				rubiHealth.TakeDamage (damage);
			}
		}
	}

	void FixedUpdate () {
		lifeSpan -= Time.deltaTime;
		if (lifeSpan <= 0) {
			dying = true;
			GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
			Destroy(gameObject);
		}

		if (GetComponent<Rigidbody2D> ().position.x < RubiControllerScript.rubiControl.gameObject.transform.position.x) {
			velocity.x += speed;
		} else {
			velocity.x -= speed;
		}
		if (GetComponent<Rigidbody2D> ().position.y < RubiControllerScript.rubiControl.gameObject.transform.position.y) {
			velocity.y += speed;
		} else {
			velocity.y -= speed;
		}
		GetComponent<Rigidbody2D> ().velocity = new Vector2 (velocity.x, velocity.y);
	}
}
