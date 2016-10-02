using UnityEngine;
using System.Collections.Generic;

public class PlayerBehaviour : MonoBehaviour {

	private Transform _transform;
	private SpriteRenderer sprite;
	private float colorTransformFactor;

	private GameController gameController;

	private EscortBehaviour escort;

	public List<KeyCode> shootButton;
	
	// Movement modifier applied to directional movement
	public float playerSpeed = 400.0f;

	// Bullets
	public Transform bullet;
	public float bulletDistance = .2f; // relative to the center of the ship
	public float timeBetweenFires = .3f; // "Cooloff" time
	private float timeTilNextFire = 0.0f;
	public float secondsDisabledAfterHit = 2f;
	public float timeUntilReenable;
	public AudioClip fireSound;
	public AudioClip moveEscortSound;

	// Use this for initialization
	void Start () {
		this._transform = this.GetComponent<Transform>();
		this.sprite = (SpriteRenderer)GetComponent<SpriteRenderer>();
		this.gameController = GameObject
				.FindGameObjectWithTag("GameController")
				.GetComponent<GameController>();
		this.escort = GameObject
				.FindGameObjectWithTag("Escort")
				.GetComponent<EscortBehaviour>();
		this.reset();
	}
	
	// Update is called once per frame
	void Update () {
		// Process keyboard shooting input
		if (Input.GetAxis ("Fire1") == 1) this._fire();
		timeTilNextFire -= Time.deltaTime;
		if (timeUntilReenable > 0) {
			timeUntilReenable -= Time.deltaTime;
			colorTransformFactor = (secondsDisabledAfterHit - timeUntilReenable) / secondsDisabledAfterHit;
			sprite.color = new Color(1, Mathf.Clamp(colorTransformFactor, 0, 1),Mathf.Clamp(colorTransformFactor, 0, 1),1);
			this.transform.position = new Vector2 ( UnityEngine.Random.Range(-232f, -228f), this.transform.position.y);
		} else {
			this._move ();
			this.transform.position = new Vector2 ( -230f, this.transform.position.y);
		}
	}

	private void reset(){
		this._transform.position = new Vector2(-230f, 0);
		this.timeUntilReenable = 0;
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

	private void disableTemporarily(){
		timeUntilReenable = secondsDisabledAfterHit;
	}

	private void _fire(){
		if (timeTilNextFire <= 0 && timeUntilReenable <= 0){
			// this.transform.position is the transform for the current player's position
			Vector3 bulletPos = this.transform.position;

			// Determine the laser angle
			float rotationAngle = transform.localEulerAngles.z - 90;

			// Calculate the position of the laser in front of the ship
			bulletPos.x += (Mathf.Cos((rotationAngle) * Mathf.Deg2Rad) * - bulletDistance);
			bulletPos.y += (Mathf.Sin((rotationAngle) * Mathf.Deg2Rad) * - bulletDistance);
			// Create the bullet instance
			Instantiate (bullet, bulletPos, this.transform.rotation);
			gameController.audioSource.PlayOneShot(this.fireSound);
			timeTilNextFire = timeBetweenFires;
		}
	}

	public void destroyPlayer() {
		Destroy(this.gameObject);
	}

	private void OnTriggerEnter2D(Collider2D other) {
		// Ignore bullet collisions (which happen when player fires)
		if (other.gameObject.CompareTag ("Bullet")) return;
		
		if (other.gameObject.CompareTag ("Enemy")) {
			other.SendMessage("DestroyEnemy");
			disableTemporarily();
		}
		if (other.gameObject.CompareTag ("Powerup")) {
			other.SendMessage("DestroyPowerup");
			gameController.updateLivesCount(+1);
		}
		if (other.gameObject.CompareTag ("EscortDownMover") || other.gameObject.CompareTag ("EscortUpMover")) {
			other.SendMessage("DestroyPowerup");
			escort.moveEscort(other.gameObject);
			gameController.audioSource.PlayOneShot(this.moveEscortSound);
		}
	}
}
