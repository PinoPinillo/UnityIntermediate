using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
	// Rotation speed
	public float speed;
	
	// Update is called once per frame
	void Update ()
	{
		// Rotates this GameObject in the y axis
		transform.Rotate (Vector3.up * speed * Time.deltaTime);
	}
}
