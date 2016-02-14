using UnityEngine;
using System.Collections;

public class AlphaParticleController : MonoBehaviour {
	
	public float velocity;
	public float lifeSpan;
	public int damage;
	private float currLife;
	public float angle;
	private GameObject player;
	RubiControllerScript playerMovement; 
	private bool dying = false;
	public bool hit = false;
	
	private int partHorizontal = 0;
	private int partVertical = 0;
	
	Animator anim;
	
	RubiEnergy rubiEnergy;
	
	void Start () {
		anim = gameObject.GetComponent<Animator> ();
		player = GameObject.FindGameObjectWithTag( "Player" );
		currLife = lifeSpan;
		if (Input.GetAxisRaw ("Vertical") > 0 ) {
			partVertical = 1;
			partHorizontal = 0;
			GetComponent<Rigidbody2D>().transform.Rotate(0f, 0f, 90f);
		} else if (Input.GetAxisRaw ("Vertical") < 0 && RubiControllerScript.rubiControl.grounded == false) {
			partVertical = -1;
			partHorizontal = 0;
			GetComponent<Rigidbody2D>().transform.Rotate(0f, 0f, 270f);
		} else {
			if (RubiControllerScript.rubiControl.facingRight != true) {
				Vector3 theScale = GetComponent<Rigidbody2D>().transform.localScale;
				theScale.x *= -1;
				GetComponent<Rigidbody2D>().transform.localScale = theScale;
				partHorizontal = -1;
			}
			if (RubiControllerScript.rubiControl.facingRight == true) {
				partHorizontal = 1;
			}
		}
		currLife = lifeSpan;
	}
	
	void FixedUpdate () {
		if (!dying && !hit) {
			GetComponent<Rigidbody2D>().velocity = new Vector2 (velocity * partHorizontal, velocity * partVertical);
			currLife -= Time.deltaTime;
			if (currLife <= 0) {
				dying = true;
				anim.SetTrigger ("Die");
			}
		}
		if (hit) {
			anim.SetTrigger ("Die");
		}
	}
	public void DestroyShot() {
		Destroy(gameObject);
	}
	void OnTriggerEnter2D (Collider2D collider) {
		if (collider.gameObject.tag != "Player") {
			hit = true;
		}
	}
}
