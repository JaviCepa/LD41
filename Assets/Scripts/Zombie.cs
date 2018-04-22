using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeCanvas.Framework;

public class Zombie : Actor
{

	public int damageOnContact = 1;
	public float attackRange = 0.5f;

	public override bool IsEnemyOf(Actor actor)
	{
		return !(actor is Zombie);
	}

	private void Update()
	{
		foreach (var overlap in Physics.OverlapSphere(transform.position, attackRange, LayerMask.GetMask("Humans")))
		{
			var human = overlap.GetComponent<Human>();
			if (human != null)
			{
				human.Damage(damageOnContact);
			}
		}
	}

	protected override void OnKillFx()
	{
		FxManager.DisplayFx("ZombieSplat", transform.position);
	}

}
