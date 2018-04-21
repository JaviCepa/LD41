using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkSpriteController : MonoBehaviour
{

	public List<Sprite> walkSprites;
	public float walkingAnimationSpeed = 1;
	public float hopHeight = 1f / 6f;
	[Range(0f, 1f)]public float walkAnimationAsymmetry = 0.5f;

	float walkingTimer = 0;
	SpritePicker spritePicker;
	Sprite standingSprite { get { return spritePicker.sprites[spritePicker.currentIndex]; } }
	Sprite walkingSprite { get { return walkSprites[spritePicker.currentIndex]; } }
	SpriteRenderer spriteRenderer { get { if (spriteRenderer_ == null) { spriteRenderer_ = GetComponent<SpriteRenderer>(); } return spriteRenderer_; } }
	SpriteRenderer spriteRenderer_;
	ActorController actorController { get { if (actorController_ == null) { actorController_ = GetComponentInParent<ActorController>(); } return actorController_; } }
	ActorController actorController_;

	void Awake()
	{
		spritePicker = GetComponent<SpritePicker>();
		walkingTimer = Random.value * 2f;
	}
	
	void LateUpdate ()
	{
		if (!actorController.isWalking)
		{
			transform.parent.localPosition = Vector3.zero;
			spriteRenderer.sprite = standingSprite;
			walkingTimer = Random.value * 2f;
		}
		else
		{
			if (walkingTimer < walkAnimationAsymmetry)
			{
				transform.parent.localPosition = Vector3.up * hopHeight;
				spriteRenderer.sprite = walkingSprite;
			}
			else if (walkingTimer < 1.0f)
			{
				transform.parent.localPosition = Vector3.zero;
				spriteRenderer.sprite = standingSprite;
			}
			else
			{
				transform.parent.localPosition = Vector3.zero;
				spriteRenderer.sprite = standingSprite;
				walkingTimer -= 1f;
			}
			walkingTimer += Time.deltaTime * walkingAnimationSpeed;
		}
	}
}
