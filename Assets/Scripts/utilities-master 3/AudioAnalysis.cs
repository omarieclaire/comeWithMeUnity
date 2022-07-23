using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioAnalysis : MonoBehaviour {

    public AudioSource audioSource;

	public List<float> audioSpectrum;
    
	public FloatListEvent floatListEvent;
	
	public int spectrumSize;

    private float[] samples;

    private void Start()
    {
        audioSpectrum = new List<float>();
        samples = new float[spectrumSize];
    }

    private void FixedUpdate()
    {
        if (audioSource.isPlaying)
        {
            if (audioSpectrum == null)
                audioSpectrum = new List<float>();
            audioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman);
	        audioSpectrum = new List<float>(samples);
	        floatListEvent.Invoke(audioSpectrum);
        }

    }
}
