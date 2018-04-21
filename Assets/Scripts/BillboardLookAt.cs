using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BillboardLookAt : MonoBehaviour
{

	private void Start()
	{
		transform.LookAt(transform.position + Camera.main.transform.forward);
	}

	void Update()
	{
		if (!Application.isPlaying)
		{
			transform.LookAt(transform.position + Camera.main.transform.forward);
		}
	}

}
