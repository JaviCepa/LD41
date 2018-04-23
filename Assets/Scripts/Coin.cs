using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Coin : MonoBehaviour
{

	bool collected = false;
	public AudioSource coinSoundSource;

	private void OnTriggerEnter(Collider other)
	{
		if (!collected && other.GetComponent<HumanPlayer>() != null)
		{
			coinSoundSource.Play();
			GameDirector.AddCoin();
			transform.DOScale(0, 0.3f).OnComplete(() => Destroy(gameObject));
			collected = true;
		}
	}

}
