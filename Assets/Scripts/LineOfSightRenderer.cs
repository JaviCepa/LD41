using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSightRenderer : MonoBehaviour {

	public float radius = 1;

	public SphereCollider attackArea;

	[Button("Update Attack Area")]
	public void UpdateAttackArea()
	{
		var lineRenderer = GetComponent<LineRenderer>();
		for (int i = 0; i < lineRenderer.positionCount; i++)
		{
			float alpha = (float) i / (float) lineRenderer.positionCount;
			float angle = alpha * 360 * Mathf.Deg2Rad;
			var direction = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
			var circlePosition = direction * radius;
			lineRenderer.SetPosition(i, circlePosition);
		}
		attackArea.radius = radius;
	}
}
