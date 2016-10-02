using UnityEngine;
using System;

public class EscortBehaviour : MonoBehaviour {

	private Transform _transform;
	private PlayerBehaviour player;

	public Transform escortExplosion;

	private GameController gameController;
	
	// Movement modifier applied to directional movement
	public float escortSpeed = 0.25f;
	public float shakeTimeAfterDamage = 1f;
	private float remainingShakeTime = 0;

	// Positions
	private int[] escortYPositions;
	private int currentPos = 1;
	private int destinationPos = 1;

	// Use this for initialization
	void Start () {
		this.escortYPositions = new int[4] { 100, 0, -100, -200 };
		this._transform = this.GetComponent<Transform>();
		this.gameController = GameObject
				.FindGameObjectWithTag("GameController")
				.GetComponent<GameController>();
		this.player = GameObject
				.FindGameObjectWithTag("Player")
				.GetComponent<PlayerBehaviour>();
		destinationPos = currentPos;
	}
	
	// Update is called once per frame
	void Update () {
		if (destinationPos != currentPos) this._move ();
		if (remainingShakeTime > 0) this.shake();
	}

	public void moveEscort(GameObject pickup) {
		if (pickup.tag == "EscortDownMover") {
			if ( (currentPos == destinationPos) || (currentPos - destinationPos < 0) ) {
				// Escort is stopped or moving down.
				destinationPos++;
			} else {
				// Escort is moving up, flip movement.
				// First, estimate which position the escort is closest to
				var closestPos = Array.IndexOf(escortYPositions, Mathf.RoundToInt(this._transform.position.y / 100) * 100);
				// Debug.Log("Y: " + this._transform.position.y + " Closest position: " + closestPos);
				currentPos = closestPos;
				destinationPos = currentPos + 1;
			}
		}
		
		if (pickup.tag == "EscortUpMover") {
			if ( (currentPos == destinationPos) || (currentPos - destinationPos > 0) ) {
				// Escort is stopped or moving up.
				destinationPos--;
			} else {
				// Escort is moving down, flip movement.
				// First, estimate which position the escort is closest to
				var closestPos = Array.IndexOf(escortYPositions, Mathf.RoundToInt(this._transform.position.y / 100) * 100);
				// Debug.Log("Y: " + this._transform.position.y + " Closest position: " + closestPos);
				currentPos = closestPos;
				destinationPos = currentPos - 1;
			}
		}

		if (destinationPos > 3) destinationPos = 3;
		if (destinationPos < 0) destinationPos = 0;

	}

	// Move the ship 
	private void _move(){
		int movementBoundary = escortYPositions[destinationPos];

		Vector3 movement = new Vector3 ();
		movement.x = 0;
		movement.y += currentPos - destinationPos;
		movement.Normalize ();

		if (movement.magnitude > 0) {
			this._transform.Translate (movement * Time.deltaTime * escortSpeed, Space.World);
		} 

		// Check if the ship reached the destination position
		if (currentPos - destinationPos > 0) {
			// Ship is moving up
			if (this._transform.position.y >= movementBoundary) {
				this._transform.position = new Vector2 (-330f, movementBoundary);
				currentPos = destinationPos;
			}
		}
		if (currentPos - destinationPos < 0) {
			// Ship is moving down
			if (this._transform.position.y <= movementBoundary) {
				this._transform.position = new Vector2 (-330f, movementBoundary);
				currentPos = destinationPos;
			}
		}
	}

	// Shake the ship (to indicate damage)
	private void shake() {
		remainingShakeTime -= Time.deltaTime;
		if (remainingShakeTime > 0) 
			this.transform.position = new Vector2 ( UnityEngine.Random.Range(-325f, -335f), this.transform.position.y);
		else this.transform.position = new Vector2 ( -330f, this.transform.position.y);
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag ("Enemy")) {
			other.SendMessage("DestroyEnemy");
			if (gameController.updateLivesCount(-1) <= 0) {
				player.destroyPlayer();
				Destroy(this.gameObject);
				if (escortExplosion) {
					GameObject exploder = ((Transform)Instantiate(escortExplosion, this.transform.position, this.transform.rotation)).gameObject;
				}
			} else {
				this.remainingShakeTime = this.shakeTimeAfterDamage;
			}
		}
	}
}
