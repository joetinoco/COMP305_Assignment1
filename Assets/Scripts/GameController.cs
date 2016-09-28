using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public Transform enemy;
	public Transform powerup;

	[Header("Enemies and difficulty")]
	public int initialLives = 5;
	public int enemiesPerWave = 3;
	public float timeBetweenEnemyIncreases = 30;
	public int increaseEnemyCountBy = 1;

	public int playerLives;
	private int currentNumberOfEnemies = 0;
	private int enemiesInCurrentWave;
	private float elapsedTimeForCurrentWave = 0;

	[Header("User Interface")]
	public Text scoreText;
	public Text livesText;
	private int playerScore = 0;

	[Header("Sounds")]
	public AudioSource audioSource;
	public AudioClip deathSound;
	public AudioClip powerupSound;

	// GAME INITIALIZATION
	void Start () {
		enemiesInCurrentWave = enemiesPerWave;
		playerLives = initialLives;
		this.audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		spawnEnemies();
		spawnPowerups();
		updateUI();
	}

	// Allow enemy objects to update the game enemy count
	public void updateEnemyCount(int delta) {
		currentNumberOfEnemies += delta;
	}

	// Allow player to update the game lives count
	public void updateLivesCount(int delta) {
		playerLives += delta;
		if (delta < 0) this.audioSource.PlayOneShot(deathSound);
		else this.audioSource.PlayOneShot(powerupSound);
	}

	// Allow player to update the score
	public void updateScore(int delta) {
		playerScore += delta;
	}

	// Ensure a correct number of enemies in the screen
	private void spawnEnemies(){
		if (currentNumberOfEnemies < enemiesInCurrentWave){
			Instantiate(enemy);
			currentNumberOfEnemies++;
		}
		// Increase the number of enemies for the current wave if enough time has passed
		elapsedTimeForCurrentWave += Time.deltaTime;
		if (elapsedTimeForCurrentWave > timeBetweenEnemyIncreases){
			elapsedTimeForCurrentWave = 0;
			enemiesInCurrentWave += increaseEnemyCountBy;
		}
	}

	// Ensure a correct number of enemies in the screen
	private void spawnPowerups(){
		if (GameObject.FindWithTag("Powerup") == null){
			Instantiate(powerup);
		}
	}

	private void updateUI(){
		scoreText.text = "Score: " + playerScore;
		livesText.text = "Lives: " + playerLives;
	}

}
