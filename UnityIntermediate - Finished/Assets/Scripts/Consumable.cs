using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConsumableType
{
	Ammo,
	Health
}

public class Consumable : MonoBehaviour
{
	// The type of this consumable
	public ConsumableType type;

	// Use this for initialization
	void Start ()
	{
		// Destroys this with a delay of 15 seconds
		Destroy (this.gameObject, 15f);
	}

	void OnTriggerEnter (Collider other)
	{
		// If the object that collides is the player, gives health or ammo depending on the consumable type
		if (other.CompareTag ("Player")) {
			switch (type) {
			case ConsumableType.Ammo:
				// Gives 25 bullets
				GameController.playerAmmo += 25;
				// Destroys this GameObject
				Destroy (this.gameObject);
				break;
			case ConsumableType.Health:
				// Gives 25 health points
				GameController.playerHealth += 25f;
				// Destroys this GameObject
				Destroy (this.gameObject);
				break;
			default:
				break;
			}
		}	
	}
}
