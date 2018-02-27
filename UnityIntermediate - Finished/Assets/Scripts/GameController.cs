using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	// GameController instance
	public static GameController _instance;

	// Player health points
	public static float playerHealth;
	// Player ammo
	public static int playerAmmo;
	// Player score used to buy in some rounds
	public static int score;

	// Game total kills
	public static int totalKills;
	// Current round kills
	public static int roundKills;
	// Current round
	private int round;
	// Number of enemies spawned in the current round
	private int enemiesSpawned;
	// Max number of enemies per round
	public int maxEnemies;

	// The enemies GameObject to spawn
	public GameObject enemy;
	// Spawning points
	public Transform[] spawnPoints;
	// The time between spawns
	public float timeToSpawn;

	// Can the player buy objects?
	private bool canBuy;
	// The temple door used to change the scene
	public GameObject templeDoor;
	// The time the player has to buy
	public float timeToBuy;
	// The time elapsed in the buying round
	private float buyTimer;

	// The player GameObject
	private GameObject player;

	// The current health image
	public Image healthImage;
	// The current ammo text
	public Text ammoText;
	// The score text
	public Text scoreText;
	// The round text
	public Text roundText;
	// The timer displayed in the buying round
	public Text buyTimerText;
	// The game over menu
	public GameObject gameOverMenu;
	// This game kills text
	public Text killsText;
	// This game final round text
	public Text finalRoundText;
	// Kills highscore text
	public Text maxKillsText;
	// Round highscore text
	public Text maxRoundText;
	// The restart button
	public Button restartButton;
	// The exit button
	public Button exitButton;

	void Awake ()
	{
		// Don't destroy on load this GameObject
		DontDestroyOnLoad (transform.gameObject);

		// If the instance is null, sets this as the instance. Otherwise, destroys this GameObject
		if (_instance == null) {
			_instance = this;
		} else {
			Destroy (this.gameObject);
		}
	}

	// Use this for initialization
	void Start ()
	{
		// Sets the kills highscore to 0 if there is no highscore yet
		if (!PlayerPrefs.HasKey ("KillsHighscore")) {
			PlayerPrefs.SetInt ("KillsHighscore", 0);
		}
		// Sets the round highscore to 0 if there is no highscore yet
		if (!PlayerPrefs.HasKey ("RoundHighscore")) {
			PlayerPrefs.SetInt ("RoundHighscore", 0);
		}

		// Finds the player GameObject
		player = GameObject.FindGameObjectWithTag ("Player");

		// Adds the method called when pressing the restart button
		restartButton.onClick.AddListener (RestartPressed);
		// Adds the method called when pressing the exit button
		exitButton.onClick.AddListener (ExitPressed);

		// Initializes some values
		SetUp ();

		// Starts spawning enemies with a delay between spawns
		InvokeRepeating ("SpawnEnemy", timeToSpawn, timeToSpawn);
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Sets the health image fill amount depending on player health points
		healthImage.fillAmount = playerHealth / 100f;
		// Sets the ammo text
		ammoText.text = playerAmmo + " / 100";
		// Sets the score text
		scoreText.text = string.Format ("{0:0000}", Mathf.Clamp (score, 0, 9999));
		// Sets the round text
		roundText.text = string.Format ("ROUND {0:000}", Mathf.Clamp (round, 0, 999));

		if (playerHealth > 0f) {
			// Sets the player max health
			if (playerHealth >= 100f) {
				playerHealth = 100f;
			}

			// Sets the player max ammo
			if (playerAmmo >= 100) {
				playerAmmo = 100;
			}

			// If the player has killed all enemies in this round, moves on to the next one
			if (roundKills >= maxEnemies) {
				NextRound ();
			}

			// If the player can buy, starts the countdown
			if (canBuy) {
				// Increases the time elapsed in the buying round
				buyTimer += Time.deltaTime;

				// If the time elapsed is greater or equal to the time to buy, moves on to the next round
				if (buyTimer >= timeToBuy) {
					// If the player is inside the temple, loads the main scene and resets player position
					if (SceneManager.GetActiveScene ().Equals (SceneManager.GetSceneByBuildIndex (1))) {
						SceneManager.LoadScene (0);
						player.transform.position = new Vector3 (0f, 0.55f, 0f);
					}
					NextRound ();
				} else {
					// Calculates the time remaining
					float remainingTime = timeToBuy - buyTimer;
					// Sets the timer text
					buyTimerText.text = string.Format ("{0:00} : {1:00}", Mathf.FloorToInt (remainingTime / 60f),
						Mathf.FloorToInt (remainingTime % 60));
				}
			}
		}
	}

	void SpawnEnemy ()
	{
		// If the player is dead, the current round is a buying one or the enemies spawned are greater or equal to
		// the max number of enemies per round, returns
		if (playerHealth <= 0f || canBuy || enemiesSpawned >= maxEnemies)
			return;

		// Gets a random number to select the point where the enemy will be spawn
		int pointToSpawn = Random.Range (0, spawnPoints.Length);
		// Instantiate a new enemy
		Instantiate (enemy, spawnPoints [pointToSpawn].position, spawnPoints [pointToSpawn].rotation);
		// Increases the enemies spawned for this round
		enemiesSpawned++;
	}

	public void NextRound ()
	{
		// Increases the current round
		round++;
		// Resets the round enemies spawned
		enemiesSpawned = 0;
		// Resets the round kills
		roundKills = 0;

		// If the new round is multiple of 3, this is a buying round
		if (round % 3 == 0) {
			// The player can buy
			canBuy = true;
			// Enables the temple door
			templeDoor.SetActive (true);
			// Resets the buying time
			buyTimer = 0f;
			// Enables the timer text
			buyTimerText.enabled = true;
		} else {
			// The player can not buy
			canBuy = false;
			// Disables the temple door
			templeDoor.SetActive (false);
			// Disables the timer text
			buyTimerText.enabled = false;
		}
	}

	public void GameOver ()
	{
		// Disables the player GameObject
		player.SetActive (false);

		// If this game kills is greater than kills highscore, sets the new highscore
		if (totalKills > PlayerPrefs.GetInt ("KillsHighscore")) {
			PlayerPrefs.SetInt ("KillsHighscore", totalKills);
		}
		// If this game number of rounds survived is greater than rounds highscore, sets the new highscore
		if (round > PlayerPrefs.GetInt ("RoundHighscore")) {
			PlayerPrefs.SetInt ("RoundHighscore", round);
		}

		// Enables the game over menu
		gameOverMenu.SetActive (true);
		// Sets the texts to show the final scores
		killsText.text = string.Format ("TOTAL KILLS: {0:0000}", Mathf.Clamp (totalKills, 0, 9999));
		finalRoundText.text = string.Format ("FINAL ROUND: {0:000}", Mathf.Clamp (round, 0, 999));
		maxKillsText.text = string.Format ("MAX KILLS: {0:0000}", Mathf.Clamp (PlayerPrefs.GetInt ("KillsHighscore"), 0, 9999));
		maxRoundText.text = string.Format ("HIGHEST ROUND: {0:000}", Mathf.Clamp (PlayerPrefs.GetInt ("RoundHighscore"), 0, 999));
	}

	// Initializes all values needed to start the game
	void SetUp ()
	{
		playerHealth = 100f;
		playerAmmo = 100;
		score = 0;
		totalKills = 0;

		roundKills = 0;
		round = 1;
		enemiesSpawned = 0;

		canBuy = false;
		templeDoor.SetActive (false);
		buyTimer = 0f;
		buyTimerText.enabled = false;

		player.transform.position = new Vector3 (0f, 0.55f, 0f);
		player.SetActive (true);

		gameOverMenu.SetActive (false);
	}

	// Resets the game
	void RestartPressed ()
	{
		SetUp ();
	}

	// Close the game
	void ExitPressed ()
	{
		Application.Quit ();
	}
}
