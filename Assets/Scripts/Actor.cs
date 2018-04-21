using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Actor : MonoBehaviour
{

	public float walkSpeed = 1;
	public int health = 1;
	public Weapon currentWeapon { get { return GetComponentInChildren<Weapon>(); } }

	bool frozen = false;

	[HideInInspector] public bool isWalking { get { return !navMeshAgent.isStopped; } set { navMeshAgent.isStopped = !value; } }

	NavMeshAgent navMeshAgent;

	public void Attack(Actor actor)
	{
		var weapon = currentWeapon;
		if (weapon != null)
		{
			currentWeapon.Use(actor);
		}
	}

	private void OnTriggerEnter(Collider other)
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

	protected void LookLeft()
	{
		transform.GetChild(0).localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
	}

	protected void LookRight()
	{
		transform.GetChild(0).localScale = new Vector3(+1, transform.localScale.y, transform.localScale.z);
	}

	[Button("Attack")]
	public void Attack()
	{
		if (currentWeapon != null)
		{
			currentWeapon.Use();
		}
	}

	private void LateUpdate()
	{
		var screenHorizontalMove = Vector3.Dot(navMeshAgent.velocity, Camera.main.transform.right);
		float deadZone = 0.1f;
		if (screenHorizontalMove > deadZone) { LookRight(); }
		if (screenHorizontalMove < -deadZone) { LookLeft(); }
	}

	public void Walk(Vector3 direction)
	{
		if (!frozen)
		{
			navMeshAgent.velocity = new Vector3(direction.x, 0, direction.z) * walkSpeed;
			isWalking = true;
		}
	}

	public void StopWalking()
	{
		isWalking = false;
	}
}
