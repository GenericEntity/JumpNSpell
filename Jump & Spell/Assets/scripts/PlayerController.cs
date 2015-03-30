using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
	public float maxSpeed = 5F;

	public float jumpForce = 600F;

	bool grounded = false;
	public Transform groundCheck; // makes detecting the ground simpler
	float groundRadius = 0.03F;
	public LayerMask whatIsGround;

	// Animating physics, so use FixedUpdate
	void FixedUpdate () 
	{
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);

		float move = Input.GetAxis("Horizontal");
		GetComponent<Rigidbody2D>().velocity = new Vector2(move * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);

	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space) && grounded)
			GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce));
	}

	void OnTriggerExit2D(Collider2D col)
	{
		Application.LoadLevel(Application.loadedLevel);
	}
}
