using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActorController : ActorController
{

	public HumanInput humanInput;

	void Update ()
	{
		var horizontal = Input.GetAxisRaw("Horizontal");
		var vertical = Input.GetAxisRaw("Vertical");
		var fire = Input.GetAxisRaw("Fire1");

		var groundHorizontal = Camera.main.transform.right;
		var groundVertical = Vector3.ProjectOnPlane(Camera.main.transform.up, Vector3.up).normalized;

		Vector3 walkVector = groundHorizontal * horizontal + groundVertical * vertical;

		Walk(walkVector);

		if (horizontal == 0 && vertical == 0)
		{
			StopWalking();
		}

		if (fire > 0)
		{
			Attack();
		}
	}

}
