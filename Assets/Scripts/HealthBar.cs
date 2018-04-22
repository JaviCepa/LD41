using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

	public Gradient gradient;
	Image image;

	private void Awake()
	{
		image = GetComponentInChildren<Image>();
		image.enabled = false;
	}

	public void UpdateHealthBar(float fillAmount)
	{
		fillAmount = Mathf.Clamp01(fillAmount);
		if (fillAmount == 1 || fillAmount == 0)
		{
			image.enabled = false;
		}
		else
		{
			image.enabled = true;
			image.fillAmount = fillAmount;
			image.color = gradient.Evaluate(fillAmount);
		}
	}

}
