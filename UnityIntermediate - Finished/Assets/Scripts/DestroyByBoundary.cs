using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByBoundary : MonoBehaviour
{
	void OnTriggerExit (Collider other)
	{
		// Destroys the objects that exit from this collider
		Destroy (other.gameObject);
	}
}
