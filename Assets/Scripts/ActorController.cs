using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

[SelectionBase]
public abstract class ActorController : MonoBehaviour
{

	public float walkSpeed = 1f;

	DOTweenAnimation attackAnimation;

	[HideInInspector] public bool isWalking { get { return !navMeshAgent.isStopped; } set { navMeshAgent.isStopped = !value; } }

	NavMeshAgent navMeshAgent;

	private void Awake()
	{
		navMeshAgent = GetComponent<NavMeshAgent>();
		attackAnimation = GetComponentInChildren<DOTweenAnimation>();
	}

	protected void Update()
	{
		if (isWalking)
		{
			var screenHorizontalMove = Vector3.Dot(navMeshAgent.destination-transform.position, Camera.main.transform.right);

			float deadZone = 0.1f;
			if (screenHorizontalMove > deadZone) { LookRight(); }
			if (screenHorizontalMove < -deadZone) { LookLeft(); }
		}
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
		attackAnimation.DORestart();
	}

	public void Walk(Vector3 direction)
	{
		navMeshAgent.SetDestination(transform.position + new Vector3(direction.x, 0, direction.z) * 2f);
		isWalking = true;
	}

	public void StopWalking()
	{
		isWalking = false;
	}

}