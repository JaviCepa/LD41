using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeCanvas.Framework;

public class Zombie : Actor
{

	public int damageOnContact = 1;
	public float sightRadius = 3f;
	float attackRange = 1f;

	float wanderTimer = 0;

	Dictionary<ZombieState, System.Action> stateActions;

	public ZombieState currentState = ZombieState.Wandering;

	Human currentTarget;

	bool hasDestination = false;

	public GameObject coinPrefab;

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
	}

	private void Update()
	{
		stateActions[currentState].Invoke();
	}

	private void Advance()
	{
		if (GameDirector.instance.humanPlayer != null)
		{
			var destination = GameDirector.instance.humanPlayer.transform.position;
			Debug.DrawLine(transform.position, destination);
			var delta = destination - transform.position;
			navMeshAgent.SetDestination(destination);
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
		if (currentTarget == null)
		{
			currentState = ZombieState.Wandering;
			return;
		}

		Debug.DrawLine(transform.position, currentTarget.transform.position);
		Walk(currentTarget.transform.position - transform.position);
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
		FxManager.SpawnZombieBits(transform.position, Mathf.RoundToInt(maxHealth/3f));
		if (coinPrefab!=null)
		{
			Instantiate(coinPrefab, transform.position, coinPrefab.transform.rotation);
		}
	}

}


public enum ZombieState { Wandering, Chasing, Advancing }