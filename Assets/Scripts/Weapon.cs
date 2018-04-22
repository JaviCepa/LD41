using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class Weapon : MonoBehaviour
{

	[Title("Weapon")]
	public string weaponName;
	public int damage;
	public float range;
	public float rateOfFire = 1f;

	[Title("Bullet")]
	public float bulletSpeed = 5f;
	public Sprite bulletSprite;
	public GameObject bulletPrefab;

	[HideInInspector] public Transform cannonPosition;
	[HideInInspector] public Sprite weaponSprite { get { return GetComponentInChildren<SpriteRenderer>().sprite; } }
	public SphereCollider pickupCollider;
	RandomSoundClip randomSoundClip;

	Actor currentOwner = null;

	DOTweenAnimation attackAnimation;
	
	float lastUseTime = 0;

	bool isReady { get { return Time.time - lastUseTime > rateOfFire && currentOwner != null; } }

	private void Awake()
	{
		attackAnimation = GetComponentInChildren<DOTweenAnimation>();
		randomSoundClip = GetComponentInChildren<RandomSoundClip>();
		var cannon = GetComponentInChildren<WeaponCannon>();
		if (cannon != null)
		{
			cannonPosition = cannon.transform;
		}
		else
		{
			cannonPosition = transform;
		}
	}

	public void Pickup(Actor newOwner)
	{
		if (currentOwner == null && !newOwner.frozen)
		{
			var ownerHand = newOwner.GetComponentInChildren<Hand>();
			if (ownerHand != null && ownerHand.isReady)
			{
				lastUseTime = Time.time;
				currentOwner = newOwner;
				currentOwner.LookRight();
				currentOwner.Freeze();
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
					ownerHand.PickSomething();
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
		currentOwner = null;
		transform.SetParent(null);
		var sequence = DOTween.Sequence();
		sequence.Append(transform.DOMoveY(0.1f, 0.3f).SetEase(Ease.InBack));
		sequence.Join(transform.DOMoveX(transform.position.x + Random.value - 0.5f, 0.3f).SetEase(Ease.InBack));
		sequence.Join(transform.DOMoveZ(transform.position.z + Random.value - 0.5f, 0.3f).SetEase(Ease.InBack));
	}

	public void Use(Actor target = null)
	{
		if (isReady && target != null)
		{
			currentOwner.LookTo(target.transform.position, 5);
			currentOwner.Freeze();
			attackAnimation.DORestart();
			lastUseTime = Time.time;
			Vector3 spawnPosition = transform.position;
			if (cannonPosition != null)
			{
				spawnPosition = cannonPosition.position;
			}

			randomSoundClip.PlayRandomClip();

			var newBulletObject = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity) as GameObject;
			var newBullet = newBulletObject.GetComponent<Bullet>();
			newBullet.Initialize(bulletSprite, target.transform.position, bulletSpeed, damage);
			Invoke("Unfreeze", Mathf.Min(rateOfFire * 0.25f, 0.5f));
		}
	}

	void Unfreeze()
	{
		if (currentOwner != null)
		{
			currentOwner.Unfreeze();
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
