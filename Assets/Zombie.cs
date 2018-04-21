using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Actor
{
	public override bool IsEnemyOf(Actor actor)
	{
		return !(actor is Zombie);
	}
}
