using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CameraController : MonoBehaviour
{

	public Transform target;
	public float distance = 30;
	
	void LateUpdate ()
	{
		if (target)
		{
			UpdatePosition();
		}
	}

	[Button("Reposition")]
	private void UpdatePosition()
	{
		transform.position = target.position - distance * transform.forward;
	}
}
