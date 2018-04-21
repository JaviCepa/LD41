using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Human : Actor
{

	float lookoutDistance = 2f;

	public override bool IsEnemyOf(Actor actor)
	{
		return !(actor is Human);
	}

	private void LateUpdate()
	{
		GameObject nearestObject = null;
		float minDistance = float.MaxValue;
		foreach (var zombieCollider in Physics.OverlapSphere(transform.position, lookoutDistance, LayerMask.GetMask("Zombies")))
		{
			if (!zombieCollider.isTrigger)
			{
				float distance = (zombieCollider.transform.position - transform.position).magnitude;
				if (distance < minDistance)
				{
					minDistance = distance;
					nearestObject = zombieCollider.gameObject;
				}
			}
		}
		if (nearestObject != null)
		{
			LookTo(nearestObject.transform.position);
		}
	}

}
