using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDistanceInterval : MonoBehaviour
{
    [System.Serializable]
    public class IntervalAudio
    {
        public AudioSource audioSource;
        public List<AudioClip> clips;
        public List<float> pitches;
        public float distance = 2;
        public float sequenceLength;
        public float chanceOfPlay = 50f;
        [HideInInspector]
	    public List<int> sequence;
	    [HideInInspector]
	    public List<float> pitchSequence;
        [HideInInspector]
        public Vector3 lastPoint = Vector3.zero;
        [HideInInspector]
        public int currentSequence = 0;
    }
	[TagSelector]
	public string targetString;
	public Transform target;
    public List<IntervalAudio> intervalAudios;


    private void Start()
	{
		if(target==null){
			target = GameObject.FindGameObjectWithTag(targetString).transform;
		}
        foreach(IntervalAudio interval in intervalAudios)
        {
        	string debug = "";
	        interval.sequence = new List<int>();
	        interval.pitchSequence = new List<float>();
            for(int i = 0; i < interval.sequenceLength; i++)
            {
                if (Random.Range(0, 100) < interval.chanceOfPlay)
                {
	                interval.sequence.Add(Random.Range(0, interval.clips.Count));
	                interval.pitchSequence.Add(interval.pitches[Random.Range(0, interval.pitches.Count)]);
                }
                else
                {
                    interval.sequence.Add(-1);
                }
	            debug+=interval.sequence[i] + " ";

            }
	        
        }
    }
    private void Update()
	{

        foreach(IntervalAudio interval in intervalAudios)
        {
            if (Vector3.Distance(target.position, interval.lastPoint) > interval.distance)
            {
                interval.lastPoint = target.position;
                //interval.audioSource.pitch = interval.pitches[Random.Range(0, interval.pitches.Count)];
                if (interval.sequence[interval.currentSequence] != -1)
                {
	                AudioClip clip = interval.clips[interval.sequence[interval.currentSequence]];
	                interval.audioSource.pitch = interval.pitchSequence[interval.currentSequence];
                    interval.audioSource.PlayOneShot(clip);
                }

                if (interval.currentSequence >= interval.sequenceLength-1)
                {
                    interval.currentSequence = 0;
                }
                else
                {
                    interval.currentSequence++;
                }
            }

        }

    }
}
