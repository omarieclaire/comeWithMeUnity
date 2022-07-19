using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomAudioClip : MonoBehaviour
{
	public List<AudioClip> audioClips;
	public AudioSource audioSource;
	private AudioClip audioClip;
	
	void Start()
	{
		//audioClip = audioClips[Random.Range(0, audioClips.Count)];
		//audioSource.clip = audioClip;
		//audioSource.Play();
	}

}
