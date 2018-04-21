using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour {

	public float radius { get { return radius_; } set { radius_ = value; UpdateAttackArea(); } }
	float radius_ = 0;

	public SphereCollider attackArea;

	Actor actor;

	private void Awake()
	{
		actor = GetComponentInParent<Actor>();
	}
	
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
