using UnityEngine;
using System.Collections;

public class MoveEscortPickupBehaviour : MonoBehaviour {

	private int _speed = 5;
	private Transform _transform;

	public int Speed {
		get {
			return this._speed;
		} 
		set {
			this._speed = value;
		}
	}

	// Use this for initialization
	void Start () {
		this._transform = this.GetComponent<Transform>();
		this._reset ();
	}

	// Update is called once per frame
	void Update () {
		this._move ();
		this._checkBounds ();
	}

	// Move the powerup left
	private void _move() {
		Vector2 newPosition = this._transform.position;
		newPosition.x -= this._speed;
		this._transform.position = newPosition;
	}

	// Check if the powerup is gone off screen
	private void _checkBounds() {
		if (this._transform.position.x <= -430f) {
			this._reset ();
		}

	}

	// Reset powerup to starting position
	private void _reset() {
		this._speed = Random.Range(5,10);
		this._transform.position = new Vector2 (430f, Random.Range(-228f, 270f));
	}

	public void DestroyPowerup(){
		Destroy(gameObject);
	}
}
