using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{

	public float maxIntensity = 2f;

	Light sunLight;
	AmplifyColorEffect amplifyColor;

	void Start ()
	{
		sunLight = GetComponent<Light>();
		amplifyColor = FindObjectOfType<AmplifyColorEffect>();
	}
	
	void Update ()
	{
		if (enabled)
		{
			float phase = Mathf.Clamp01(Vector3.Dot(transform.forward, Vector3.down));
			sunLight.intensity = phase * maxIntensity;
			if (amplifyColor != null)
			{
				amplifyColor.BlendAmount = 1f - phase;
			}
		}
	}
}
