using UnityEngine;
using System.Collections;

public class MithPuppetController : MonoBehaviour {

	private Animator anim;

	private bool facingRight = true;
	public bool phasing = false;

	public SpriteRenderer render;

	void Start () {
		anim = GetComponent<Animator> ();
		render = GetComponent<SpriteRenderer> ();
	}

	void FixedUpdate() {
		if (GameVars.vars.mith1 == true && phasing == false) {
			phasing = true;
			anim.SetBool ("Phasing", true);
		}
	}

	void Dissapear () {
		render.color = new Color (1f, 1f, 1f, 0f);
		DestroyThis ();
	}

	void Flip()
	{
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
	
	public void DestroyThis()
	{
		Destroy(gameObject);
	}
}
