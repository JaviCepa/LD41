using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {

	SpritePicker spritePicker;

	void Start ()
	{
		spritePicker = GetComponent<SpritePicker>();
	}

	[Button("Upgrade")]
	public void Upgrade()
	{
		spritePicker.currentIndex++;
		spritePicker.currentIndex = Mathf.Clamp(spritePicker.currentIndex, 0, spritePicker.sprites.Count-1);
		spritePicker.UpdateSprite();
	}
}
