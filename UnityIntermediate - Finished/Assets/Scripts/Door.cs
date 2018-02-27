using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
	// The scene we are going to load
	public int sceneIndex;

	// Use this for initialization
	void Start ()
	{
		// Sets the templeDoor
		GameController._instance.templeDoor = this.gameObject;	
	}

	void OnTriggerEnter (Collider other)
	{
		// If the object that enters the trigger is the player, load another scene
		if (other.CompareTag ("Player")) {
			SceneManager.LoadScene (sceneIndex);
			// Resets the player position
			other.transform.position = new Vector3 (0f, 0.55f, 0f);
		}	
	}
}
