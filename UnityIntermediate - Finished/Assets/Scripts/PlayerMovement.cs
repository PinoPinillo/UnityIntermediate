using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Bounds of player movement
[System.Serializable]
public class Boundary
{
	public float xMin, xMax, zMin, zMax;
}

public class PlayerMovement : MonoBehaviour
{
	// Movement speed
	public float speed;
	// Movement bounds
	public Boundary bounds;

	// Layer mask used to aim with the mouse cursor
	private int layerMask;
	// Used to get information back from the raycast
	private RaycastHit hit;
	// Rotation applied to the player to aim
	private Quaternion rot;

	// The player Animator component
	private Animator anim;
	// The player Rigidbody component
	private Rigidbody rb;

	// Use this for intialization
	void Start ()
	{
		// Creates the layer mask with the Terrain layer
		layerMask = LayerMask.GetMask ("Terrain");

		// Gets the Animator
		anim = GetComponent <Animator> ();
		// Gets the Rigidbody
		rb = GetComponent <Rigidbody> ();

		// Initializes the rotation
		rot = Quaternion.identity;
	}

	void FixedUpdate ()
	{
		// Gets the movement vector
		float moveHorizontal = Input.GetAxisRaw ("Horizontal");
		float moveVertical = Input.GetAxisRaw ("Vertical");
		Vector3 mov = new Vector3 (-moveHorizontal, 0f, -moveVertical);
		mov = transform.position + (mov * speed * Time.deltaTime);

		// Limits the player movement
		mov = new Vector3 (
			Mathf.Clamp (mov.x, bounds.xMin, bounds.xMax), 
			0.55f,
			Mathf.Clamp (mov.z, bounds.zMin, bounds.zMax)
		);

		// Applies the movement
		rb.MovePosition (mov);

		// If the player is moving, performs the walk animation. Otherwise, performs the idle animation
		if (moveHorizontal != 0f || moveVertical != 0f) {
			anim.SetBool ("isWalking", true);
		} else {
			anim.SetBool ("isWalking", false);
		}

		// Calls this method to aim with the mouse cursor
		LookToMouse ();
	}

	void LookToMouse ()
	{
		// Creates a new ray from the mouse cursor with the direction of the camera
		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);

		// If the ray hits the floor, gets the new point to look at
		if (Physics.Raycast (camRay, out hit, Mathf.Infinity, layerMask)) {
			Vector3 playerToMouse = hit.point - transform.position;
			playerToMouse.y = 0f;
			rot = Quaternion.LookRotation (playerToMouse);
		}

		// Sets the new rotation
		rb.MoveRotation (rot);
	}
}