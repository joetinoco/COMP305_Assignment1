using UnityEngine;
using System.Collections;

public class BulletBehaviour : MonoBehaviour {

	// How fast will it move
	public float speed;

	// How much damage does it deal
	public int damage;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (Vector3.right * Time.deltaTime * speed);
		// Destroy the bullets that flew off the screen
		if (transform.position.x > 400) {
			Destroy (gameObject);
		}
	}

	// Allow collided objects to destroy the bullet that hit them
	public void DestroyBullet(){
		Destroy (gameObject);
	}
}
