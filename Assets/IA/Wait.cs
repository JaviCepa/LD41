using NodeCanvas.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wait : ConditionTask
{

	public BBParameter<float> waitTime = 1f;
	float currentTime = 0f;

	protected override bool OnCheck()
	{
		currentTime += Time.deltaTime;
		if (currentTime > waitTime.value)
		{
			currentTime = 0f;
			return true;
		}
		else
		{
			return false;
		}
	}

}
