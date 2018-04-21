using NodeCanvas.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindNearestHuman : ActionTask
{

	protected override void OnExecute()
	{
		GameObject target = null;
		var humans = GameObject.FindObjectsOfType<Human>();
		float minDistance = float.MaxValue;
		foreach (var human in humans)
		{
			var distance = (human.transform.position - agent.transform.position).magnitude;
			if (distance < minDistance)
			{
				minDistance = distance;
				target = human.gameObject;
			}
		}

		if (target != null)
		{
			blackboard.SetValue("target", target);
			EndAction(true);
		}
	}
	
}
