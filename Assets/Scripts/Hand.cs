using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{

	float delayBetweenPicks = 1f;

	[HideInInspector] public bool isReady { get { return Time.time - lastPickTime > delayBetweenPicks; } }

	float lastPickTime = -10f;

	public void PickSomething()
	{
		lastPickTime = Time.time;
	}

}
