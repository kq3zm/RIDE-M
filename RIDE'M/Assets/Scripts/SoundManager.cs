using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
	public AudioSource efxSource; 
	public AudioSource crowd;
	public AudioSource co2; 
	public AudioSource btn;
	public AudioSource obs;
	public static SoundManager instance = null;
	public float delay = 7.0f;

	public void PlaySingle(AudioClip clip)
	{
		//Set the clip of our efxSource audio source to the clip passed in as a parameter.
		efxSource.clip = clip;
		efxSource.PlayDelayed (delay);
	}

	public void PlayBooSound (AudioClip clip)
	{
		//Set the clip of our efxSource audio source to the clip passed in as a parameter.
		crowd.clip = clip;
		crowd.volume = 0.2f;
		crowd.Play ();
	}

	public void PlayCheerSound (AudioClip clip)
	{
		//Set the clip of our efxSource audio source to the clip passed in as a parameter.
		crowd.clip = clip;
		crowd.volume = 0.8f;
		crowd.Play ();
	}

	public void PlayCO2Sound (AudioClip clip)
	{
		//Set the clip of our efxSource audio source to the clip passed in as a parameter.
		co2.clip = clip;
		co2.volume = 0.6f;
		co2.Play ();
	}

	public void PlayButtonSound (AudioClip clip)
	{
		//Set the clip of our efxSource audio source to the clip passed in as a parameter.
		btn.clip = clip;
		btn.volume = 0.6f;
		btn.Play ();
	}

	public void PlayObstacleSound (AudioClip clip)
	{
		//Set the clip of our efxSource audio source to the clip passed in as a parameter.
		obs.clip = clip;
		obs.volume = 1f;
		obs.Play ();
	}

	public bool isPlaying (AudioClip clip)
	{
		crowd.clip = clip;
		return crowd.isPlaying;
	}

	public void StopSingle() {
		efxSource.Stop ();
	}
}