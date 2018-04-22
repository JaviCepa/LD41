using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanAIController : Human
{

	public HumanState currentState;

	Dictionary<HumanState, System.Action> stateActions;

	public Transform currentTarget;
	[HideInInspector]public Actor currentlyFollowing;

	float followDistance;
	float fleeDistance = 3f;

	public RandomSoundClip wihSounds;

	private void Start()
	{
		stateActions = new Dictionary<HumanState, System.Action>();
		stateActions.Add(HumanState.Idle, Idle);
		stateActions.Add(HumanState.Following, Follow);
		stateActions.Add(HumanState.Guarding, Guard);
		stateActions.Add(HumanState.Patroling, Patrol);
		stateActions.Add(HumanState.Building, Build);

		followDistance = Random.Range(1f, 2f);
	}

	private void Update()
	{
		stateActions[currentState].Invoke();
	}

	new protected void OnTriggerStay(Collider other)
	{
		base.OnTriggerStay(other);
		switch (currentState)
		{
			case HumanState.Undefined:
				break;
			case HumanState.Idle:
				TryToFollow(other);
				break;
			case HumanState.Following:
				break;
			case HumanState.Guarding:

				break;
			case HumanState.Patroling:

				break;
			case HumanState.Building:
				break;
			default:
				break;
		}
	}

	public void UseActionPoint(ActionPoint actionPoint)
	{
		if (actionPoint.isAvailable && currentWeapon != null && currentWeapon.range >= actionPoint.requiredRange)
		{
			currentlyFollowing = null;
			transform.SetParent(actionPoint.transform, true);
			currentTarget = actionPoint.destinationPoint;
			navMeshAgent.SetDestination(actionPoint.destinationPoint.position);
			currentState = HumanState.Guarding;
		}
	}

	public void LeaveActionPoint()
	{

	}

	void TryToFollow(Collider other)
	{
		var humanPlayer = other.GetComponent<HumanPlayer>();
		if (humanPlayer != null)
		{
			currentlyFollowing = humanPlayer;
			currentTarget = humanPlayer.transform;
			currentState = HumanState.Following;
			humanPlayer.FollowerJoined(this);
			transform.DOPunchScale(0.1f*Vector3.one, 0.25f);
			wihSounds.PlayRandomClip();
		}
	}

	private void Idle()
	{
		Transform nearestZombie = null;

		foreach (var collider in Physics.OverlapSphere(transform.position, fleeDistance, LayerMask.GetMask("Zombies")))
		{
			var zombie = collider.GetComponent<Zombie>();
			if (zombie != null && nearestZombie == null || ((zombie.transform.position - transform.position).magnitude < (nearestZombie.transform.position - transform.position).magnitude))
			{
				nearestZombie = zombie.transform;
			}
		}

		if (nearestZombie == null)
		{
			StopWalking();
		}
		else
		{
			var zombieDirection = nearestZombie.transform.position - transform.position;
			Walk(transform.position-zombieDirection);
		}
		
	}

	private void Follow()
	{
		if (currentTarget == null)
		{
			currentState = HumanState.Idle;
			return;
		}

		if ((transform.position - currentTarget.transform.position).magnitude > followDistance)
		{
			Walk(currentTarget.gameObject);
		}
		else
		{
			if (!currentlyFollowing.isWalking || currentlyFollowing == null)
			{
				StopWalking();
			}
		}
	}

	private void Build()
	{
		
	}

	private void Patrol()
	{
		
	}

	private void Guard()
	{
		if ((transform.position - currentTarget.transform.position).magnitude > 0.1f)
		{
			
		}
		else
		{
			StopWalking();
		}
	}


}


public enum HumanState { Undefined, Idle, Following, Guarding, Patroling, Building }