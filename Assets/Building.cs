using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using System;

[ExecuteInEditMode]
public class Building : MonoBehaviour {

	public Transform topPart;
	public Ease ease;

	public float height = 10;
	public float easeTime = 0.5f;
	public SpriteRenderer spriteRenderer1;
	public SpriteRenderer spriteRenderer2;

	float startHeight = 10;
	Vector3 startPosition;

	bool isVisible = true;

	void Start ()
	{
		startPosition = topPart.localPosition;
		startHeight = height;
	}
	
	void Update ()
	{
		if (!Application.isPlaying)
		{
			UpdateBuilding();
		}
		else
		{
			var center = Camera.main.transform.position + Camera.main.transform.up*0.5f;
			var delta = transform.position - center;
			if (Vector3.Dot(delta, Camera.main.transform.up) > 0)
			{
				ShowBuilding();
			}
			else
			{
				HideBuilding();
			}

		}

	}

	private void UpdateBuilding()
	{
		topPart.transform.localScale = new Vector3(topPart.transform.localScale.x, height, topPart.transform.localScale.z);
		topPart.transform.localPosition = new Vector3(topPart.transform.localPosition.x, height * 0.5f + 0.5f, topPart.transform.localPosition.z);
		spriteRenderer1.size = new Vector2(spriteRenderer1.size.x, height);
		spriteRenderer2.size = new Vector2(spriteRenderer2.size.x, height);
		spriteRenderer1.transform.position = new Vector3(spriteRenderer1.transform.position.x, topPart.transform.position.y, spriteRenderer1.transform.position.z);
		spriteRenderer2.transform.position = new Vector3(spriteRenderer2.transform.position.x, topPart.transform.position.y, spriteRenderer2.transform.position.z);
	}

	[Button("Hide")]
	public void HideBuilding()
	{
		if (isVisible)
		{
			DOTween.To(() => height, (x) => height = x, 0.5f, easeTime).OnUpdate(() => UpdateBuilding()).OnComplete(() => isVisible = false).SetEase(ease);
		}
	}

	[Button("Show")]
	public void ShowBuilding()
	{
		if (!isVisible)
		{
			DOTween.To(() => height, (x) => height = x, startHeight, easeTime).OnUpdate(() => UpdateBuilding()).OnComplete(() => isVisible = true).SetEase(ease);
		}
	}
}
