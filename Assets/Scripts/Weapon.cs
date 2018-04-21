using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Weapon : MonoBehaviour
{

	public string weaponName;
	public int damage;
	public float range;
	public float rateOfFire = 1f;

	[HideInInspector] public Sprite weaponSprite { get { return GetComponentInChildren<SpriteRenderer>().sprite; } }
	public SphereCollider pickupCollider;

	Actor currentOwner = null;

	DOTweenAnimation attackAnimation;

	float lastDropTime = -10;
	float lastUseTime = 0;

	bool isReady { get { return Time.time - lastUseTime > 1f / rateOfFire; } }

	private void Awake()
	{
		attackAnimation = GetComponentInChildren<DOTweenAnimation>();
	}

	public void Pickup(Actor newOwner)
	{
		if (currentOwner == null && Time.time - lastDropTime > 2f)
		{
			var ownerHand = newOwner.GetComponentInChildren<Hand>();
			if (ownerHand != null)
			{
				currentOwner = newOwner;
				currentOwner.Freeze();
				currentOwner.LookRight();
				pickupCollider.enabled = false;
				if (currentOwner.currentWeapon != null)
				{
					currentOwner.currentWeapon.Drop();
				}
				var sequence = DOTween.Sequence();
				sequence
				.Append(transform.DOMoveX(ownerHand.transform.position.x, 0.3f).SetEase(Ease.Linear))
				.Join(transform.DOMoveZ(ownerHand.transform.position.z, 0.3f).SetEase(Ease.Linear))
				.Join(transform.DOMoveY(ownerHand.transform.position.y, 0.3f).SetEase(Ease.OutBack))
				.OnComplete(() =>
				{
					transform.SetParent(ownerHand.transform);
					transform.localPosition = Vector3.zero;
					transform.rotation = Quaternion.identity;
					transform.localScale = new Vector3(ownerHand.transform.parent.localScale.x, transform.localScale.y, transform.localScale.z);
					currentOwner.GetComponentInChildren<AttackRange>().radius = range;
					currentOwner.Unfreeze();
				}
				);
			}
		}
	}

	public void Drop()
	{
		currentOwner.GetComponentInChildren<AttackRange>().radius = 0;
		pickupCollider.enabled = true;
		lastDropTime = Time.time;
		currentOwner = null;
		transform.SetParent(null);
		var sequence = DOTween.Sequence();
		sequence.Append(transform.DOMoveY(0.1f, 0.3f).SetEase(Ease.InBack));
		sequence.Join(transform.DOMoveX(transform.position.x + Random.value - 0.5f, 0.3f).SetEase(Ease.InBack));
		sequence.Join(transform.DOMoveZ(transform.position.z + Random.value - 0.5f, 0.3f).SetEase(Ease.InBack));
	}

	public void Use(Actor target = null)
	{
		if (isReady)
		{
			//TODO: Do damage
			currentOwner.LookTo(target.transform.position);
			attackAnimation.DORestart();
			lastUseTime = Time.time;
		}
	}

	private void OnDrawGizmosSelected()
	{
		int points = 32;
		var previousPosition = Vector3.zero;
		for (int i = 0; i < points; i++)
		{
			float alpha = (float) i / (float) points;
			float angle = alpha * 360 * Mathf.Deg2Rad;
			var direction = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
			var circlePosition = direction * range;
			var currentPosition = transform.position + circlePosition;
			Gizmos.color = Color.yellow * 0.75f;
			Gizmos.DrawLine(transform.position, currentPosition);
			if (previousPosition != Vector3.zero)
			{
				Gizmos.color = Color.white;
				Gizmos.DrawLine(currentPosition, previousPosition);
			}
			previousPosition = currentPosition;
		}
	}
}
