using UnityEngine;
using System.Collections;

public class Racket : MonoBehaviour {

	public ExamplePongLogic logic;

	// Use this for initialization
	void Start () {
	}

	void OnCollisionEnter2D (Collision2D col) {

		if (col.gameObject.GetComponent<Rigidbody2D> () != null) {

			float hitPos = (col.transform.position.y - transform.position.y) / (GetComponent<Collider2D> ().bounds.size.y / 2);
			Vector3 hitPos2 = col.GetContact(0).point;
			var hitDir2 = (hitPos2 - col.transform.GetChild(0).position).normalized;
			float hitDir = 1f;

			if (col.relativeVelocity.x > 0) {
				hitDir = -1f;
			}

			Vector2 dir = new Vector2 (hitDir, hitPos).normalized;
			Vector2 dir2 = new Vector2(hitDir2.x, hitDir2.y);
			col.gameObject.GetComponent<Rigidbody2D> ().velocity = dir2 * logic.ballSpeed;

		}
	}
}
