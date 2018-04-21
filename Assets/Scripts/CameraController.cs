using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

	public Transform target;
	public float dampening = 0.5f;

	void Start ()
	{
		
	}
	
	void LateUpdate ()
	{
		if (target)
		{
			var delta = target.position-transform.position;
			var error = Vector3.ProjectOnPlane(delta, transform.forward);
			transform.position += error * dampening;
		}
	}
}
