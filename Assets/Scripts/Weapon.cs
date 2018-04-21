using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Weapon : MonoBehaviour
{

	Actor currentOwner = null;

	DOTweenAnimation attackAnimation;

	float lastDropTime = 0;

	private void Awake()
	{
		attackAnimation = GetComponentInChildren<DOTweenAnimation>();
	}

	public void Pickup(Actor newOwner)
	{
		if (currentOwner == null && Time.time - lastDropTime > 3f)
		{
			var ownerHand = newOwner.GetComponentInChildren<Hand>();
			if (ownerHand != null)
			{
				currentOwner = newOwner;
				currentOwner.Freeze();
				if (currentOwner.currentWeapon != null) { currentOwner.currentWeapon.Drop(); }
				var sequence = DOTween.Sequence();
				sequence
				.Append(transform.DOMoveX(ownerHand.transform.position.x, 0.3f).SetEase(Ease.Linear))
				.Join(transform.DOMoveZ(ownerHand.transform.position.z, 0.3f).SetEase(Ease.Linear))
				.Join(transform.DOMoveY(ownerHand.transform.position.y, 0.3f).SetEase(Ease.OutBack))
				.OnComplete(() =>
				{
					transform.SetParent(ownerHand.transform);
					transform.localPosition = Vector3.zero;
					currentOwner.Unfreeze();
				}
				);
			}
		}
	}

	public void Drop()
	{
		currentOwner = null;
		lastDropTime = Time.time;
	}

	public void Use(Actor target = null)
	{
		attackAnimation.DORestart();
	}
}
