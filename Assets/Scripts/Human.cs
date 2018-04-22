using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Human : Actor
{

	float defaultLookoutDistance = 2f;

	public override bool IsEnemyOf(Actor actor)
	{
		return !(actor is Human);
	}

	private void Update()
	{
		GameObject nearestObject = null;
		float minDistance = float.MaxValue;
		var lookOutDistance = (currentWeapon != null) ? defaultLookoutDistance + currentWeapon.range : defaultLookoutDistance;

		foreach (var zombieCollider in Physics.OverlapSphere(transform.position, lookOutDistance, LayerMask.GetMask("Zombies")))
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
			LookTo(nearestObject.transform.position, 5);
		}
	}

}
