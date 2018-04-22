using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Actor : MonoBehaviour
{

	public float walkSpeed = 1;
	public int health = 1;
	public Weapon currentWeapon { get { return GetComponentInChildren<Weapon>(); } }
	int currentLookPriority = 0;

	[HideInInspector] public bool frozen = false;

	[HideInInspector] public bool isWalking { get { return !navMeshAgent.isStopped; } set { navMeshAgent.isStopped = !value; } }

	NavMeshAgent navMeshAgent;
	bool isAlive { get { return health > 0; } }

	public abstract bool IsEnemyOf(Actor actor);

	public void Attack(Actor actor)
	{
		var weapon = currentWeapon;
		if (weapon != null)
		{
			currentWeapon.Use(actor);
		}
	}
	
	protected void OnTriggerStay(Collider other)
	{
		var targetWeapon = other.GetComponent<Weapon>();
		if (targetWeapon != null)
		{
			targetWeapon.Pickup(this);
		}
	}

	public void Freeze()
	{
		frozen = true;
	}

	public void Unfreeze()
	{
		frozen = false;
	}

	private void Awake()
	{
		navMeshAgent = GetComponent<NavMeshAgent>();
	}

	[Button("Attack")]
	public void Attack()
	{
		if (currentWeapon != null)
		{
			currentWeapon.Use();
		}
	}

	public void Walk(GameObject target)
	{
		Walk((target.transform.position - transform.position).normalized);
	}

	public void Walk(Vector3 direction)
	{
		if (!frozen)
		{
			navMeshAgent.velocity = new Vector3(direction.x, 0, direction.z).normalized * walkSpeed;
			isWalking = true;
			LookTo(transform.position + navMeshAgent.velocity, 0);
		}
	}

	public void StopWalking()
	{
		isWalking = false;
	}

	public void Damage(int amount)
	{
		if (isAlive)
		{
			health -= amount;
			if (health <= 0)
			{
				Kill();
			}
		}
	}

	private void Kill()
	{
		Destroy(gameObject);
	}

	public void LookTo(Vector3 lookPosition, int lookPriority = 0)
	{
		if (!frozen && (lookPriority > currentLookPriority))
		{
			Debug.DrawLine(transform.position, lookPosition);
			var screenHorizontalProjection = Vector3.Dot(lookPosition - transform.position, Camera.main.transform.right);
			float deadZone = 0.1f;
			if (screenHorizontalProjection > deadZone) { LookRight(); }
			if (screenHorizontalProjection < -deadZone) { LookLeft(); }
			currentLookPriority = lookPriority;
		}
	}

	private void LateUpdate()
	{
		currentLookPriority = -1;
	}

	public void LookLeft()
	{
		if (!frozen)
		{
			transform.GetChild(0).localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
		}
	}

	public void LookRight()
	{

		if (!frozen)
		{
			transform.GetChild(0).localScale = new Vector3(+1, transform.localScale.y, transform.localScale.z);
		}
	}
}
