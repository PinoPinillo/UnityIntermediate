using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
	// This object price
	public int price;
	// The type of consumable
	public ConsumableType type;
	// The information text displayed when the player is in range
	public Text infoText;
	// Is the player in range?
	private bool isInRange;
	// Sound played when the player buy this object
	private AudioSource sound;

	// Use this for initialization
	void Start ()
	{
		// Hides the information text
		infoText.enabled = false;
		// Sets the default message
		infoText.text = "Press B to buy this \nfor " + price + " coins";
		// The player is not in range
		isInRange = false;
		// Gets the audio source
		sound = GetComponent<AudioSource> ();
	}

	// Update is called once per frame
	void Update ()
	{
		// If the player is in range, display the information text. Otherwise, hides it
		if (isInRange) {
			infoText.enabled = true;
			// If the player has enough coins, shows the price of this object and allows the player buying it.
			// Otherwise, shows a message saying that the player can not buy this object
			if (GameController.score >= price) {
				infoText.text = "Press B to buy this \nfor " + price + " coins";
				// When pressing the B key, buy this object
				if (Input.GetKeyDown (KeyCode.B)) {
					switch (type) {
					// If the object is ammo, refills the player ammo
					case ConsumableType.Ammo:
						GameController.playerAmmo = 100;
						// Decreases the player score by this object price
						GameController.score -= price;
						// Plays the sound
						sound.Play ();
						break;
					// If the object is health, refills the player health points
					case ConsumableType.Health:
						GameController.playerHealth = 100f;
						// Decreases the player score by this object price
						GameController.score -= price;
						// Plays the sound
						sound.Play ();
						break;
					default:
						break;
					}
				}
			} else {
				infoText.text = "You need " + price + " coins to buy this";
			}
		} else {
			infoText.enabled = false;
		}
	}

	void OnTriggerEnter (Collider other)
	{
		// If the other object is the player, the player is in range
		if (other.CompareTag ("Player")) {
			isInRange = true;
		}
	}

	void OnTriggerExit (Collider other)
	{
		// If the other object is the player, the player is not in range
		if (other.CompareTag ("Player")) {
			isInRange = false;
		}
	}
}
