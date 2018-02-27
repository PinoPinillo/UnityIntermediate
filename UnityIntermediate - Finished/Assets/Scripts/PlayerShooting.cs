using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
	// The point where the bullets are going to be spawned
	public GameObject shotSpawn;
	// The bullets to spawn
	public GameObject shot;
	// The time between shots
	public float fireRate;
	// Time elapsed since last shot
	private float timeElapsed = 0f;
	
	// Update is called once per frame
	void Update ()
	{
		// Increases the timeElapsed
		timeElapsed += Time.deltaTime;

		// If the player has ammo, has passed enough time since last shot and the player is pressing the fire button,
		// shoots and resets the timeElapsed value
		if (GameController.playerAmmo > 0 && timeElapsed >= fireRate && Input.GetButton ("Fire1")) {
			timeElapsed = 0f;
			Instantiate (shot, shotSpawn.transform.position, shotSpawn.transform.rotation);
			// Decreases by one the player ammo
			GameController.playerAmmo--;
		}
	}
}
