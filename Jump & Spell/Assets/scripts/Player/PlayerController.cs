using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
	public float maxSpeed = 5F;

	public float jumpForce = 700F;

	private bool grounded = false;
	private float groundRadius = 0.03F;
	public Transform groundCheck; // makes detecting the ground simpler
	public LayerMask whatIsGround;

	// Animating physics, so use FixedUpdate
	void FixedUpdate () 
	{
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);

		float move = Input.GetAxis("Horizontal");
		GetComponent<Rigidbody2D>().velocity = new Vector2(
			move * maxSpeed, 
			GetComponent<Rigidbody2D>().velocity.y);
	}

	void Update()
	{
		if(Input.GetButtonDown("jump") && grounded)
			GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce));
	}
}
