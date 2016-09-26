using UnityEngine;
using System.Collections;

public class BackgroundBehaviour : MonoBehaviour {

	// Simple BG scroller

	private int _speed = 3;
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
		this._speed = 3;
	}
	
	// Update is called once per frame
	void Update () {
		this._move ();
		this._checkBounds ();
	}

	// Move the space left
	private void _move() {
		Vector2 newPosition = this._transform.position;
		newPosition.x -= this._speed;

		this._transform.position = newPosition;
	}

	// Check if the space is gone off screen
	private void _checkBounds() {

		if (this._transform.position.x <= -891f) {
			this._reset ();
		}

	}

	// Reset bg to starting position
	private void _reset() {
		this._transform.position = new Vector2 (891f, 0f);
	}
}
