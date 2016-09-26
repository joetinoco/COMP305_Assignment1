using UnityEngine;
using System.Collections.Generic;

public class PlayerBehaviour : MonoBehaviour {

	private Transform _transform;

	private GameController gameController;

	public List<KeyCode> shootButton;
	
	// Movement modifier applied to directional movement
	public float playerSpeed = 400.0f;

	// Bullets
	public Transform bullet;
	public float bulletDistance = .2f; // relative to the center of the ship
	public float timeBetweenFires = .3f; // "Cooloff" time
	private float timeTilNextFire = 0.0f;

	// Use this for initialization
	void Start () {
		this._transform = this.GetComponent<Transform>();
		this.gameController = GameObject
				.FindGameObjectWithTag("GameController")
				.GetComponent<GameController>();
		this.reset();
	}
	
	// Update is called once per frame
	void Update () {
		this._move ();
		// Process keyboard shooting input
		if (Input.GetAxis ("Fire1") == 1) this._fire();
		timeTilNextFire -= Time.deltaTime;
	}

	private void reset(){
		this._transform.position = new Vector2(-230f, 0);
	}

	// Move the ship 
	private void _move(){

		Vector3 movement = new Vector3 ();

		// Read input
		movement.y += Input.GetAxis ("Vertical");
		movement.x = 0;
		movement.Normalize ();

		if (movement.magnitude > 0) {
			// There is new input, set the ship in motion
			this._transform.Translate (movement * Time.deltaTime * playerSpeed, Space.World);
		} else {
		}
		// Process mouse movement (if any)
		//this._transform.position = new Vector2 (-230f, Mathf.Clamp(Input.mousePosition.y - 300f, -280f, 230f));
		
		// Check boundaries
		this._transform.position = new Vector2 (-230f, Mathf.Clamp(this._transform.position.y, -280f, 230f));
	}

	private void _fire(){
		if (timeTilNextFire <= 0){
			// this.transform.position is the transform for the current player's position
			Vector3 bulletPos = this.transform.position;

			// Determine the laser angle
			float rotationAngle = transform.localEulerAngles.z - 90;

			// Calculate the position of the laser in front of the ship
			bulletPos.x += (Mathf.Cos((rotationAngle) * Mathf.Deg2Rad) * - bulletDistance);
			bulletPos.y += (Mathf.Sin((rotationAngle) * Mathf.Deg2Rad) * - bulletDistance);
			// Create the bullet instance
			Instantiate (bullet, bulletPos, this.transform.rotation);
			timeTilNextFire = timeBetweenFires;
		}
	}

	private void OnTriggerEnter2D(Collider2D other) {
		// Ignore bullet collisions (which happen when player fires)
		if (other.gameObject.CompareTag ("Bullet")) return;
		Debug.Log("Player collided with something");
		if (other.gameObject.CompareTag ("Enemy")) {
			Debug.Log ("Enemy hit player!");
			gameController.updateLivesCount(-1);
			//this.reset();
		}
		if (other.gameObject.CompareTag ("Powerup")) {
			Debug.Log ("Picked up powerup!");
			other.SendMessage("DestroyPowerup");
			gameController.updateLivesCount(+1);
		}
	}
}
