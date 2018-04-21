using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritePicker : MonoBehaviour
{

	public bool randomizeAtStart = true;
	public List<Sprite> sprites;
	public Gradient colors;
	public int currentIndex;

	SpriteRenderer spriteRenderer { get { if (spriteRenderer_ == null) { spriteRenderer_ = GetComponent<SpriteRenderer>(); } return spriteRenderer_; } }
	SpriteRenderer spriteRenderer_;

	private void Start()
	{
		if (randomizeAtStart)
		{
			PickRandomSprite();
			PickRandomColor();
		}
	}

	[Button("Randomize Sprite")]
	public void PickRandomSprite()
	{
		currentIndex = Random.Range(0, sprites.Count);
		UpdateSprite();
	}

	[Button("Randomize Color")]
	public void PickRandomColor()
	{
		spriteRenderer.material.SetColor("_Color", colors.Evaluate(Random.value));
	}
	
	public void UpdateSprite()
	{
		spriteRenderer.sprite = sprites[currentIndex];
	}
}
