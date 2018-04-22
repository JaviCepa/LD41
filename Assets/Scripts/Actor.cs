using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public abstract class Actor : MonoBehaviour
{

	public float walkSpeed = 1;
	public int maxHealth = 10;
	public Weapon currentWeapon { get { return GetComponentInChildren<Weapon>(); } }
	public float invulnerabilityTime = 0;
	public RandomSoundClip hurtSounds;

	int currentLookPriority = 0;

	public int health { get { return currentHealth_; } set { currentHealth_ = value; healthBar.UpdateHealthBar((float)currentHealth_ / (float)maxHealth); } }
	[ReadOnly]public int currentHealth_;

	HealthBar healthBar;

	[HideInInspector] public bool frozen = false;

	[HideInInspector] public bool isWalking { get { return !navMeshAgent.isStopped; } set { navMeshAgent.isStopped = !value; } }

	protected NavMeshAgent navMeshAgent;
	bool isVulnerable { get { return (Time.time - lastDamageTime) > invulnerabilityTime; } }
	float lastDamageTime = 0;

	bool isAlive { get { return health > 0; } }

	public abstract bool IsEnemyOf(Actor actor);

	private void Awake()
	{
		navMeshAgent = GetComponent<NavMeshAgent>();
		var newObject = Instantiate(GameDirector.instance.healthBarPrefab, transform);
		newObject.transform.position = transform.position + Vector3.up * 1.5f;
		healthBar = newObject.GetComponent<HealthBar>();
		health = maxHealth;
		navMeshAgent.speed = walkSpeed;
	}

	protected void OnTriggerStay(Collider other)
	{
		var targetWeapon = other.GetComponent<Weapon>();
		if (targetWeapon != null)
		{
			targetWeapon.Pickup(this);
		}
	}

	public void Attack(Actor actor)
	{
		var weapon = currentWeapon;
		if (weapon != null)
		{
			currentWeapon.Use(actor);
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
		if (target != null)
		{
			Walk((target.transform.position - transform.position).normalized);
		}
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

	public bool Damage(int amount)
	{
		if (isAlive && isVulnerable)
		{
			health -= amount;
			lastDamageTime = Time.time;
			if (hurtSounds!=null)
			{
				hurtSounds.PlayRandomClip();
			}
			if (health <= 0)
			{
				Kill();
			}
			return true;
		}
		return false;
	}

	void Kill()
	{
		float killTime = 0.15f;
		transform.DOScale(0, killTime).SetEase(Ease.InBack);
		OnKillFx();
		Destroy(gameObject, killTime);
	}

	protected virtual void OnKillFx()
	{

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
