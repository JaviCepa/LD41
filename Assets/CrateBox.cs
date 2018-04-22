using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class CrateBox : MonoBehaviour {

	public DOTweenAnimation rotationAnimation;
	public DOTweenAnimation[] openAnimations;

	public List<ItemChances> contents;

	bool isOpen = false;

	GameObject PickRandomItem()
	{
		float totalWeight = 0;
		foreach (var item in contents)
		{
			totalWeight += item.weight;
		}

		float pickedValue = totalWeight * Random.value;
		float currentValue = 0;
		foreach (var item in contents)
		{
			if (currentValue >= pickedValue && pickedValue < currentValue + item.weight)
			{
				return item.itemPrefab;
			}
			else
			{
				currentValue += item.weight;
			}
		}
		return null;
	}

	private void OnTriggerEnter(Collider other)
	{
		Open();
	}

	[Button("Open")]
	public void Open()
	{
		if (!isOpen)
		{
			foreach (var animation in openAnimations)
			{
				animation.DORestart();
			}
			rotationAnimation.DOPause();

			var itemToSpawn = PickRandomItem();
			Instantiate(itemToSpawn, transform.position, Quaternion.identity);

			isOpen = true;
		}
	}

	[Button("Close")]
	public void Close()
	{
		if (isOpen)
		{
			foreach (var animation in openAnimations)
			{
				animation.DORewind();
			}
			rotationAnimation.DOPlay();
			isOpen = false;
		}
	}

}

[System.Serializable]
public class ItemChances
{
	[HorizontalGroup]
	public int weight = 1;
	[HorizontalGroup]
	public GameObject itemPrefab;
	
}
