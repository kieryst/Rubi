using UnityEngine;
using System.Collections;

public class WeakBlockController : MonoBehaviour {

	private AlphaParticleController part;
	public GameObject boom;
	public int boomCounter;
	public float boomDelay;

	void OnTriggerEnter2D (Collider2D collider) {
		if (collider.gameObject.tag == "AlphaParticle") {
			part = collider.gameObject.GetComponent <AlphaParticleController> ();
			part.hit = true;
			Destroy(gameObject.GetComponent<Renderer>());
			Destroy(gameObject.GetComponent<Collider2D>());
			StartCoroutine (Die());
		}
	}
	private IEnumerator Die() {
		for (int i = 0; i < boomCounter; ++i) {
			GameObject splosion = Instantiate (boom, gameObject.transform.position, Quaternion.identity) as GameObject;
			splosion.transform.localScale = new Vector3 (1.5f, 1.5f, 1.5f);
			splosion.transform.position = new Vector2 (splosion.transform.position.x + Random.Range (-0.2f, 0.2f), splosion.transform.position.y + Random.Range (-0.2f, 0.2f));
			yield return new WaitForSeconds(boomDelay);
		}
		Destroy (gameObject);
	}
}
