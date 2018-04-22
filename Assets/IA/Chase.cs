using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeCanvas.Framework;

public class Chase : ActionTask<Actor>
{

	public BBParameter<GameObject> target;
	public BBParameter<float> stopAtDistance = 0.1f;
	
	protected override void OnExecute()
	{
		if (target.value == null)
		{
			EndAction(false);
			return;
		}
		var delta = target.value.transform.position - agent.transform.position;
		if (delta.magnitude < stopAtDistance.value)
		{
			EndAction(true);
			return;
		}
	}

	protected override void OnUpdate()
	{
		agent.Walk(target.value);
	}

}
