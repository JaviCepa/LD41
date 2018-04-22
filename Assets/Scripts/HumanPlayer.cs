using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanPlayer : Human
{

	public Actor actor { get { if (actor_ == null) { actor_ = GetComponent<Actor>(); }; return actor_; } }
	Actor actor_;

	public List<HumanAIController> followers;

	void Update ()
	{
		var horizontal = Input.GetAxisRaw("Horizontal");
		var vertical = Input.GetAxisRaw("Vertical");
		var fire = Input.GetAxisRaw("Fire1");

		var groundHorizontal = Camera.main.transform.right;
		var groundVertical = Vector3.ProjectOnPlane(Camera.main.transform.up, Vector3.up).normalized;

		Vector3 walkVector = groundHorizontal * horizontal + groundVertical * vertical;

		if (walkVector.magnitude > 0)
		{
			actor.Walk(walkVector);
		}
		else
		{
			actor.StopWalking();
		}

		if (fire > 0)
		{
			actor.Attack();
		}
	}

	public void FollowerJoined(HumanAIController human)
	{
		followers.Add(human);
	}

	public void AssignFollower(ActionPoint actionPoint)
	{
		var follower = GetRandomFollower();
		if (follower != null)
		{
			followers.Remove(follower);
			follower.UseActionPoint(actionPoint);
		}
	}

	private HumanAIController GetRandomFollower()
	{
		if (followers.Count == 0) { return null; }

		return followers[Random.Range(0, followers.Count)];
	}
}
