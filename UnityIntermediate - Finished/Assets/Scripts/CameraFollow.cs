using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	// Transform to follow
	private Transform player;
	// Factor applied to follow the player with a smooth movement
	public float lerpFactor;
	// Offset applied to follow the player
	public Vector3 offset;

	void Start ()
	{
		// Finds the player GameObject and gets its Transform
		player = GameObject.FindGameObjectWithTag ("Player").transform;
	}

	void FixedUpdate ()
	{
		// Follows the player
		transform.position = Vector3.Lerp (transform.position, player.position + offset, lerpFactor * Time.deltaTime);
	}
}
