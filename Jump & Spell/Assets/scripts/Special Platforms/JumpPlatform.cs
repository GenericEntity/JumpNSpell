using UnityEngine;
using System.Collections;

public class JumpPlatform : MonoBehaviour
{
	public float jumpMagnitude = 20F;

	public void ControllerEnter2D(CharacterController2D controller)
	{
		controller.SetVerticalForce(jumpMagnitude);
	}
}
