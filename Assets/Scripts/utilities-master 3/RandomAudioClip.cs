using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAudioClip : MonoBehaviour
{
	[SerializeField]
	private List<AudioClip> audioClips;
	public List<AudioClip> AudioClips{
		get{
			return audioClips;
		} set{
			audioClips = value;
		}
	}
	[SerializeField]
	private AudioClip selectedAudioClip;
	public AudioClip SelectedAudioClip{
		get{
			return selectedAudioClip;
		} set{
			selectedAudioClip = value;
		}
	}
	
	public AudioClipEvent outputRandomClip;
	
	public void GenerateRandomClip(){
		SelectedAudioClip = AudioClips[Random.Range(0,AudioClips.Count)];
		outputRandomClip.Invoke(SelectedAudioClip);
	}
	
}
