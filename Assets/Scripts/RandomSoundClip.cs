using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSoundClip : MonoBehaviour {

	public float pitchVariation = 0.1f;
	public List<AudioClip> audioClips;
	public bool playOnStart = false;

	AudioSource audioSource;

	void Start ()
	{
		audioSource = GetComponent<AudioSource>();
		Randomize();
		if (playOnStart)
		{
			PlayClip();
		}
	}

	public void PlayRandomClip()
	{
		Randomize();
		PlayClip();
	}

	void Randomize()
	{
		if (audioClips.Count > 0 && audioSource != null)
		{
			audioSource.clip = audioClips[Random.Range(0, audioClips.Count)];
			audioSource.pitch = 1f + pitchVariation * (Random.value - 0.5f);
		}
	}

	public void PlayClip()
	{
		if (audioSource != null)
		{
			audioSource.Play();
		}
	}

}
