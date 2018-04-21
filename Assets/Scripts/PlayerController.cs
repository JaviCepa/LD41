using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

	public Actor actor { get { if (actor_ == null) { actor_ = GetComponent<Actor>(); }; return actor_; } }
	Actor actor_;

	void Update ()
	{
		var horizontal = Input.GetAxisRaw("Horizontal");
		var vertical = Input.GetAxisRaw("Vertical");
		var fire = Input.GetAxisRaw("Fire1");

		var groundHorizontal = Camera.main.transform.right;
		var groundVertical = Vector3.ProjectOnPlane(Camera.main.transform.up, Vector3.up).normalized;

		Vector3 walkVector = groundHorizontal * horizontal + groundVertical * vertical;

		actor.Walk(walkVector);

		if (horizontal == 0 && vertical == 0)
		{
			actor.StopWalking();
		}

		if (fire > 0)
		{
			actor.Attack();
		}
	}

}
