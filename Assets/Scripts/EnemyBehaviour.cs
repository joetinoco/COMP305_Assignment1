using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour {

	private GameController gameController;

	public Transform explosion;

	private int _speed = 5;
	private int _drift;
	private Transform _transform;
	public int damage = 1;

	public AudioClip hitSound;

	public int Speed {
		get {
			return this._speed;
		} 
		set {
			this._speed = value;
		}
	}

	public int Drift {
		get {
			return this._drift;
		} 
		set {
			this._drift = value;
		}
	}

	// Use this for initialization
	void Start () {
		this.gameController = GameObject
				.FindGameObjectWithTag("GameController")
				.GetComponent<GameController>();
		this._transform = this.GetComponent<Transform>();
		this._reset ();
	}

	// Update is called once per frame
	void Update () {
		this._move ();
		this._checkBounds ();
	}

	// Move the enemy left
	private void _move() {
		Vector2 newPosition = this._transform.position;
		newPosition.y += this._drift;
		newPosition.x -= this._speed;

		this._transform.position = newPosition;
	}

	// Check if the enemy is gone off screen
	private void _checkBounds() {
		if ((this._transform.position.x <= -430f) || 
		(Mathf.Clamp(this._transform.position.y,-323f,323f) != this._transform.position.y)) {
			this._reset ();
		}

	}

	// Reset enemy to starting position
	private void _reset() {
		this._speed = Random.Range(5,10);
		this._drift = Random.Range(-2,2);
		this._transform.position = new Vector2 (430f, Random.Range(-228f, 270f));
	}

	// Trigger explosion and destroy enemy
	public void DestroyEnemy(){
		Destroy(gameObject);
		gameController.audioSource.PlayOneShot(this.hitSound);
		gameController.updateEnemyCount(-1);
		gameController.updateScore(100);
		if (explosion) {
			GameObject exploder = ((Transform)Instantiate(explosion, this.transform.position, this.transform.rotation)).gameObject;
			Destroy(exploder, 2.0f);
		}
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag ("Bullet")) {
			other.SendMessage("DestroyBullet");
			damage--;
			if (damage <= 0) DestroyEnemy();
		}
	}
}
