using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour {

	public float radius { get { return radius_; } set { radius_ = value; UpdateAttackArea(); } }
	float radius_ = 0;

	public SphereCollider attackArea;
	LineRenderer lineRenderer;

	Actor actor;

	private void Awake()
	{
		actor = GetComponentInParent<Actor>();
		lineRenderer = GetComponent<LineRenderer>();
	}
	
	public void UpdateAttackArea()
	{
		float displayRadius = radius;
		if (displayRadius < 2f) { displayRadius = 0; }
		lineRenderer.enabled = displayRadius >= 2f;

		for (int i = 0; i < lineRenderer.positionCount; i++)
		{
			float alpha = (float) i / (float) lineRenderer.positionCount;
			float angle = alpha * 360 * Mathf.Deg2Rad;
			var direction = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
			var circlePosition = direction * displayRadius;
			lineRenderer.SetPosition(i, circlePosition);
		}
		attackArea.radius = radius;
	}

	private void OnTriggerStay(Collider other)
	{

		if (!other.isTrigger)
		{
			var otherActor = other.GetComponent<Actor>();

			if (otherActor != null && actor.currentWeapon != null)
			{
				if (otherActor.IsEnemyOf(actor))
				{
					actor.currentWeapon.Use(otherActor);
				}
			}
		}
	}

}
