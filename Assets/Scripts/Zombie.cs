using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeCanvas.Framework;

public class Zombie : Actor
{

	public int damageOnContact = 1;
	public float attackRange = 0.5f;
	public float sightRadius = 3f;

	float wanderTimer = 0;

	Dictionary<ZombieState, System.Action> stateActions;

	public ZombieState currentState = ZombieState.Wandering;

	Human currentTarget;

	override public bool IsEnemyOf(Actor actor)
	{
		return !(actor is Zombie);
	}

	private void Start()
	{
		stateActions = new Dictionary<ZombieState, System.Action>();
		stateActions.Add(ZombieState.Wandering, Wander);
		stateActions.Add(ZombieState.Chasing, Chase);
		stateActions.Add(ZombieState.Advancing, Advance);

		currentState = ZombieState.Advancing;
		navMeshAgent.isStopped = true;
	}

	private void Update()
	{
		stateActions[currentState].Invoke();
	}

	private void Advance()
	{
		var destination = GameDirector.instance.humanBase.transform.position;
		Debug.DrawLine(transform.position, destination);
		var delta = destination - transform.position;
		delta = new Vector3(delta.x, 0, delta.z);
		if (navMeshAgent.isStopped)
		{
			navMeshAgent.SetDestination(destination);
			navMeshAgent.isStopped = false;
		}
		WatchForHumans();
	}

	void Wander()
	{
		wanderTimer += Time.deltaTime;
		if (wanderTimer > 2f)
		{
			Walk(Random.insideUnitSphere * 5 * Random.value);
			wanderTimer = 0;
		}
		WatchForHumans();
	}

	private void WatchForHumans()
	{
		foreach (var collider in Physics.OverlapSphere(transform.position, sightRadius, LayerMask.GetMask("Humans")))
		{
			var human = collider.GetComponent<Human>();
			if (human != null)
			{
				currentTarget = human;
				currentState = ZombieState.Chasing;
			}
		}
	}

	void Chase()
	{
		Debug.DrawLine(transform.position, currentTarget.transform.position);

		if (currentTarget == null)
		{
			currentState = ZombieState.Wandering;
			return;
		}

		Walk(currentTarget.transform.position);
		if ((currentTarget.transform.position - transform.position).magnitude < attackRange)
		{
			currentTarget.Damage(damageOnContact);
		}

		if ((currentTarget.transform.position - transform.position).magnitude > 6f)
		{
			currentTarget = null;
			currentState = ZombieState.Wandering;
		}
	}

	protected override void OnKillFx()
	{
		FxManager.DisplayFx("ZombieSplat", transform.position);
	}

}


public enum ZombieState { Wandering, Chasing, Advancing }