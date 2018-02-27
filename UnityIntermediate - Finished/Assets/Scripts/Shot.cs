using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
	// The shot speed
	public float speed;

	// Use this for initialization
	void Start ()
	{
		// Sets the velocity
		GetComponent<Rigidbody> ().velocity = transform.forward * speed;
	}

	void OnTriggerEnter (Collider other)
	{
		// Ignores collisions with the GameArea and consumables
		if (other.CompareTag ("GameArea") || other.CompareTag ("Consumable")) {
			return;
		}

		// Gets the EnemyBehaviour component
		EnemyBehaviour enemyBehaviour = other.GetComponent<EnemyBehaviour> ();
		// If it has the component, calls the Hit method
		if (enemyBehaviour != null) {
			enemyBehaviour.Hit ();
		}

		// Destroys this GameObject
		Destroy (this.gameObject);
	}
}
