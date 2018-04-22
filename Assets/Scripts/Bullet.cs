using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

	public Rigidbody rb;
	Vector3 targetPosition;
	public SpriteRenderer spriteRenderer;
	int bulletDamage = 0;

	public void Initialize(Sprite bulletSprite, Vector3 target, float speed, int damage)
	{
		spriteRenderer.sprite = bulletSprite;
		target += Vector3.up * 0.5f;
		var direction = (target - transform.position).normalized;
		rb.velocity = direction * speed;
		bulletDamage = damage;
		Destroy(gameObject, 5f);
	}

	private void OnCollisionEnter(Collision collision)
	{
		collision.gameObject.SendMessage("Damage", bulletDamage, SendMessageOptions.DontRequireReceiver);
		Destroy(gameObject);
	}

}
