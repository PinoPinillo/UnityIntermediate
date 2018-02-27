using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
	// This enemy health points
	private float health = 100f;
	// Is this enemy dead?
	private bool isDead = false;

	// The player Transform to follow
	private Transform player;
	// The NavMeshAgent attached to this GameObject
	private NavMeshAgent nav;

	// Death speed used in death animation
	public float deathSpeed;
	// Time between attacks
	public float attackRate;
	// Time elapsed since last attack
	private float timeElapsed = 0f;
	// Is the player in range?
	private bool playerInRange = false;

	// Consumables that can drop when this enemy die
	public GameObject[] consumables;
	// The probability of dropping a consumable
	public float probability;

	// Use this for initialization
	void Start ()
	{
		// Finds the player GameObject and gets its Transform
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		// Gets the NavMeshAgent component attached
		nav = this.GetComponent<NavMeshAgent> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		// If the player is dead, destroys this enemy
		if (GameController.playerHealth <= 0f) {
			// Disables the NavMeshAgent
			nav.enabled = false;
			Destroy (this.gameObject);
		}

		// If this enemy is dead, performs an animation. Otherwise, follows the player and tries to kill him
		if (isDead) {
			// Disables the NavMeshAgent
			nav.enabled = false;
			// Disables the Collider attached so the bullets can pass through this enemy
			this.GetComponent<Collider> ().enabled = false;
			// Disables the Animator
			this.GetComponent<Animator> ().enabled = false;
			// Disables the Rigidbody
			this.GetComponent<Rigidbody> ().isKinematic = true;
			// Sink this enemy on the floor
			transform.Translate (-Vector3.up * deathSpeed * Time.deltaTime);
		} else {
			// Increases the time elapsed since last attack
			timeElapsed += Time.deltaTime;

			// If the NavMeshAgent is enabled, sets its destination as the player position
			if (nav.enabled)
				nav.SetDestination (player.position);

			// If the player is in range and the timeElapsed since the last attack is greater than or equal to the attackRate,
			// attacks the player
			if (playerInRange && timeElapsed >= attackRate) {
				// Decreases by 25 points the player health points
				GameController.playerHealth -= 25f;
				// If the player health points are less than or equal to 0, the game is over
				if (GameController.playerHealth <= 0f) {
					GameController._instance.GameOver ();
				}
				// Resets the time elapsed since last attack
				timeElapsed = 0f;
			}
		}
	}

	public void Hit ()
	{
		// If the enemy is dead, returns
		if (isDead)
			return;
		
		// Decreases its health by 50 points
		health -= 50f;

		// If this enemy health points are less than or equal to 0, this enemy is dead
		if (health <= 0f) {
			// Increases the score by 10 points
			GameController.score += 10;
			// Increases this round kills
			GameController.roundKills++;
			// Increases this game kills
			GameController.totalKills++;
			// Sets isDead to true
			isDead = true;

			// Gets a random number between 0 and 1
			float value = Random.Range (0f, 1f);

			// This enemy drops a random consumable depending on the probability value
			if (value >= 1 - probability) {
				Instantiate (consumables [Random.Range (0, consumables.Length)], transform.position, Quaternion.identity);
			}

			// Destroys this enemy with a delay of 2 seconds to perform the death animation
			Destroy (this.gameObject, 2f);
		}
	}

	void OnTriggerEnter (Collider other)
	{
		// If the other object is the player, the player is in range
		if (other.CompareTag ("Player")) {
			playerInRange = true;
		}
	}

	void OnTriggerExit (Collider other)
	{
		// If the other object is the player, the player is not in range
		if (other.CompareTag ("Player")) {
			playerInRange = false;
		}
	}
}
