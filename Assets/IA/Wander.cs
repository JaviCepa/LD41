using NodeCanvas.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : ActionTask<Actor>
{
	
	public BBParameter<float> minDistance = 0.5f;
	public BBParameter<float> maxDistance = 20f;
	public BBParameter<float> maxTime = 5f;

	Vector3 target;

	protected override void OnExecute()
	{
		var delta = Random.insideUnitCircle * Random.value;
		target = agent.transform.position + new Vector3(delta.x, 0, delta.y);
		
		if ((agent.transform.position - target).magnitude < 0.1f)
		{
			EndAction(true);
		}
	}

	protected override void OnUpdate()
	{
		agent.Walk(target);
		if (elapsedTime > maxTime.value)
		{
			EndAction(true);
		}
	}

}

