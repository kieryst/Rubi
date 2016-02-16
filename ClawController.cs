using UnityEngine;
using System.Collections;

public class ClawController : MonoBehaviour {

	public int claw_health = 2;
	private AlphaParticleController part;
	private bool dying;

	private bool facingRight = false;
	private bool upsideDown = false;
	private Rigidbody2D body;
	private bool attached = false;
	private BoxCollider2D collider;
	private Vector3 s;
	private Vector3 c;
	private float xScale;
	private float yScale;
	private float reach = 1f;
	private Vector2 side;
	private bool rotated = false;

	public LayerMask groundMask;

	private RaycastHit2D nw_up;
	private RaycastHit2D nw_side;
	private RaycastHit2D ne_up;
	private RaycastHit2D ne_side;
	private RaycastHit2D sw_up;
	private RaycastHit2D sw_side;
	private RaycastHit2D se_up;
	private RaycastHit2D se_side;
	public float skin = 0.05f;
	public float margin = 0.1f;
	private string dir;
	
	private RaycastHit2D permLeft;
	private RaycastHit2D permRight;	
	private RaycastHit2D permLeftGround;
	private RaycastHit2D permRightGround;

	private Animator anim;

	private bool waiting = false;
	private float currTime;
	public float minTimeBetweenActions;
	public float maxTimeBetweenActions;
	
	void Start () {
		body = gameObject.GetComponent<Rigidbody2D> ();
		collider = GetComponent<BoxCollider2D>();
		c = collider.offset;
		s = collider.size;
		xScale = transform.localScale.x;
		yScale = transform.localScale.y;
		anim = GetComponent<Animator> ();
	}

	void Update () {

		// Claw spawns should be placed near a wall. RaycastAttach will find the nearest surface and attach/orient the claw to that surface.
		if (side.x == 0.0f && side.y == 0.0f) {
			RaycastAttach ();
		}

		if (!waiting) {
			currTime = Random.Range (minTimeBetweenActions, maxTimeBetweenActions);
			waiting = true;
		} 

		// Right Ray
		permRight = Physics2D.Raycast (new Vector2 (transform.position.x + -side.y * (c.x - s.x / 2), transform.position.y + side.x * (c.y - s.y / 2)), new Vector2 (1 * side.y, 1 * -side.x), 1f, groundMask);
		Debug.DrawRay (new Vector3 (transform.position.x + -side.y*(c.x - s.x/2), transform.position.y + side.x*(c.y - s.y/2), 0), new Vector3 (1*side.y, 1*-side.x, 0), Color.white, 0);
		// Left Ray
		permLeft = Physics2D.Raycast (new Vector2 (transform.position.x + side.y * (c.x - s.x / 2), transform.position.y + -side.x * (c.y - s.y / 2)), new Vector2 (1 * -side.y, 1 * side.x), 1f, groundMask);
		Debug.DrawRay (new Vector3 (transform.position.x + side.y*(c.x - s.x/2), transform.position.y + -side.x*(c.y - s.y/2), 0), new Vector3 (1*-side.y, 1*side.x, 0), Color.white, 0);
		// Right Ground Ray
		permRightGround = Physics2D.Raycast (new Vector2 (transform.position.x + -side.y*(c.x - s.x/2) + side.x*(c.x - s.x/2), transform.position.y + side.x*(c.y - s.y/2) + side.y*(c.y - s.y/2)), new Vector2 (1*-side.x, 1*-side.y), 1f, groundMask);
		Debug.DrawRay (new Vector3 (transform.position.x + -side.y*(c.x - s.x/2) + side.x*(c.x - s.x/2), transform.position.y + side.x*(c.y - s.y/2) + side.y*(c.y - s.y/2), 0), new Vector3 (1*-side.x, 1*-side.y, 0), Color.white, 0);
		// Left Ground Ray
		permLeftGround = Physics2D.Raycast (new Vector2 (transform.position.x + side.y*(c.x - s.x/2) + side.x*(c.x - s.x/2), transform.position.y + -side.x*(c.y - s.y/2) + side.y*(c.y - s.y/2)), new Vector2 (1*-side.x, 1*-side.y), 1f, groundMask);
		Debug.DrawRay (new Vector3 (transform.position.x + side.y*(c.x - s.x/2) + side.x*(c.x - s.x/2), transform.position.y + -side.x*(c.y - s.y/2) + side.y*(c.y - s.y/2), 0), new Vector3 (1*-side.x, 1*-side.y, 0), Color.white, 0);

		if (!GameCamera.gameCamera.pseudoPause) {
			if (waiting && currTime > 0) {
				currTime -= Time.deltaTime;
			} else if (waiting && currTime <= 0) {
				waiting = false;
				float action = Random.Range (0f, 1f);
				if (action < 0.25f && (permLeft.collider == null || permLeft.fraction > margin) && (permLeftGround.collider != null)) {
					dir = "Left";
					Move (dir);
				} else if (action < 0.5f && (permRight.collider == null || permRight.fraction > margin) && (permRightGround.collider != null)) {
					dir = "Right";
					Move (dir);
				} else if (action < 0.5f && (permLeft.collider == null || permLeft.fraction > margin) && (permLeftGround.collider != null)) {
					dir = "Left";
					Move (dir);
				} else {
					Idle ();
				}
			}
			if (dir == "Right") {
				if (permRight.collider != null && permRight.fraction < margin) {
					Idle ();
				} else if (permRight.collider == null && permRightGround.collider == null) {
					Idle ();
				}
			}
			if (dir == "Left") {
				if (permLeft.collider != null && permLeft.fraction < margin) {
					Idle ();
				} else if (permLeft.collider == null && permLeftGround.collider == null) {
					Idle ();
				}
			}
			if (claw_health <= 0 && dying != true) {
				dying = true;
				anim.SetTrigger ("Die");
			}
		} else {
			Idle ();
		}
	}

	void Move(string dir) {
		anim.SetBool ("Walking", true);
		int moveDirection = 1;
		if (dir == "Left") {
			moveDirection = -1;
		}
		GetComponent<Rigidbody2D> ().velocity = new Vector3 (side.y * moveDirection, -side.x * moveDirection, 0);
	}

	void Idle() {
		GetComponent<Rigidbody2D> ().velocity = new Vector3 (0, 0, 0);
		anim.SetBool ("Walking", false);
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
	void OnTriggerStay2D (Collider2D collider) {
		if (collider.gameObject.tag == "Player") {
			if (!dying) {
				RubiHealth.rubiHealth.TakeDamage(1);
			}
		}
	}
	void TakeDamage (int damage) {
		claw_health -= damage;
	}

	void RaycastAttach() {

		// Draw 8 rays, 2 in each direction, to find a nearby surface
		Vector3 p = transform.position;
		float nw_x  = (p.x + (c.x*xScale) - s.x/2);	
		float nw_y = p.y + c.y + s.y/2;
		float se_x = (p.x + (c.x*xScale) + s.x/2);
		float se_y = p.y + c.y - s.y / 2;

		nw_up = Physics2D.Raycast(new Vector2(nw_x, nw_y), new Vector2(0, 1 * yScale), 1f, groundMask);
		nw_side = Physics2D.Raycast(new Vector2(nw_x, nw_y), new Vector2(1 * xScale * -1, 0), 1f, groundMask);

		ne_up = Physics2D.Raycast(new Vector2(se_x, nw_y), new Vector2(0, 1 * yScale), 1f, groundMask);
		ne_side = Physics2D.Raycast(new Vector2(se_x, nw_y), new Vector2(1 * xScale, 0), 1f, groundMask);

		sw_up = Physics2D.Raycast(new Vector2(nw_x, se_y), new Vector2(0, 1 * yScale * -1), 1f, groundMask);
		sw_side = Physics2D.Raycast(new Vector2(nw_x, se_y), new Vector2(1 * xScale * -1, 0), 1f, groundMask);

		se_up = Physics2D.Raycast(new Vector2(se_x, se_y), new Vector2(0, 1 * yScale * -1), 1f, groundMask);
		se_side = Physics2D.Raycast(new Vector2(se_x, se_y), new Vector2(1 * xScale, 0), 1f, groundMask);

		// Rotate the claw according to the surface location it is near to. Also set 'side' variables, which will be used to later draw and correctly position raycasting and movement direction.
		if (nw_up.collider != null || ne_up.collider != null) {
			side = new Vector2(0, -1);
			Rotate(180);
		} else if (se_up.collider != null || sw_up.collider != null) {
			side = new Vector2(0, 1);
		} else if (ne_side.collider != null || se_side.collider != null) {
			side = new Vector2(-1, 0);
			Rotate(90);
		} else if (nw_side.collider != null || sw_side.collider != null) {
			side = new Vector2(1, 0);
			Rotate(270);
		}

		Clamp (side);

	}

	// Clamp will poistion the Claw alongside a near surface.
	void Clamp(Vector2 side) {
		if (side.x != 0) {
			if (sw_side.collider == null && nw_side.collider == null) {
				if (se_side.collider == null && ne_side.collider == null) {
					Debug.Log ("No x sides found to clamp");
				} else {
					if (se_side.collider != null) {
						Vector3 thePosition = transform.position;
						thePosition.x += se_side.fraction;
						transform.position = thePosition;
					} else {
						Vector3 thePosition = transform.position;
						thePosition.x += ne_side.fraction;
						transform.position = thePosition;
					}
				}
			} else {
				if (sw_side.collider != null) {
					Vector3 thePosition = transform.position;
					thePosition.x -= sw_side.fraction;
					transform.position = thePosition;
				} else {
					Vector3 thePosition = transform.position;
					thePosition.x -= nw_side.fraction;
					transform.position = thePosition;
				}
			}
		} else if (side.y != 0) {
			if (sw_up.collider == null && se_up.collider == null) {
				if (nw_up.collider == null && ne_up.collider == null) {
					Debug.Log ("No y sides found to clamp");
				} else {
					if (nw_up.collider != null) {
						Vector3 thePosition = transform.position;
						thePosition.y += nw_up.fraction + c.y*2;
						transform.position = thePosition;
					} else {
						Vector3 thePosition = transform.position;
						thePosition.y += ne_up.fraction;
						transform.position = thePosition;
					}
				}
			} else {
				if (sw_up.collider != null) {
					Vector3 thePosition = transform.position;
					thePosition.y -= sw_up.fraction;
					transform.position = thePosition;
				} else {
					Vector3 thePosition = transform.position;
					thePosition.y -= se_up.fraction;
					transform.position = thePosition;
				}
			}
		} else {
			Debug.Log ("No sides found to clamp");
		}
	}

	void Rotate (float degrees)
	{
		rotated = !rotated;
		Quaternion theRotation = Quaternion.Euler (new Vector3 (0f, 0f, degrees));
		transform.rotation = theRotation;
	}

	public void DestroyClaw() {
		Destroy (gameObject);
	}
}
