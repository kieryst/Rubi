using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour {

	public static GameCamera gameCamera;

	private Transform target;
	public float xTrackSpeed ;
	public float yTrackSpeed ;
	public float xOffset;
	private int right = 1;

	public bool isFixed = false;
	public float fixed_x;
	public float fixed_y;

	public bool pseudoPause;

	// Singleton mode for camera. If a camera object exists on a subsequent scene, delete it and use the current one.
	void Awake () {
		if (gameCamera == null) {
			DontDestroyOnLoad (gameObject);
			gameCamera = this;
		} else if (gameCamera != this) {
			Destroy(gameObject);
		}
	}

	// Follow the player
	public void SetTarget (Transform t) {
		target = t;
	}

	// Camera is called in LateUpdate, to give all Update and FixedUpdate calls a chance to complete before the camera is moved.
	void LateUpdate () {
		float move = Input.GetAxis ("Horizontal");
		if (!pseudoPause) {
			if (move > 0)
				right = 1;
			else if (move < 0)
				right = -1;
		}
		if (target && !isFixed) {
			float x = IncrementTowards (transform.position.x, target.position.x + (right * xOffset), xTrackSpeed);
			float y = IncrementTowards (transform.position.y, target.position.y, yTrackSpeed);
			transform.position = new Vector3 (x, y, transform.position.z);
		} else if (target && isFixed) {
			float x = IncrementTowards (transform.position.x, fixed_x, xTrackSpeed);
			float y = IncrementTowards (transform.position.y, fixed_y, yTrackSpeed);
			transform.position = new Vector3 (x, y, transform.position.z);
		}
	}

	// Smooth camera motions.
	private float IncrementTowards(float n, float target, float a) {
		if (n == target) {
			return n;		
		} else {
			float dir = Mathf.Sign (target - n);
			n += a*Time.deltaTime * dir;
			return (dir == Mathf.Sign (target-n))? n:target;
		}
	}
}
