using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SignPost : MonoBehaviour {

	Canvas canvas;

	bool isVisible = false;

	void Awake()
	{
		canvas = GetComponentInChildren<Canvas>();
		canvas.transform.localScale = Vector3.zero;
	}
	
	void Update ()
	{
		bool isActive = false;
		foreach (var overlap in Physics.OverlapSphere(transform.position, 1f, LayerMask.GetMask("Humans")))
		{
			var humanPlayer = overlap.GetComponent<HumanPlayer>();
			if (humanPlayer != null)
			{
				isActive = true;
			}
		}

		if (isActive)
		{
			Display();
		}
		else
		{
			Hide();
		}
	}

	void Display()
	{
		if (!isVisible)
		{
			isVisible = true;
			canvas.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
		}
	}

	void Hide()
	{
		if (isVisible)
		{
			isVisible = false;
			canvas.transform.DOScale(0f, 0.5f).SetEase(Ease.InBack);
		}
	}
}
