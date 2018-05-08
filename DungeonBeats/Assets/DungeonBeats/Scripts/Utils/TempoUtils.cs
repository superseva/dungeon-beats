using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SynchronizerData;

public class TempoUtils : MonoBehaviour {

    public static TempoUtils instance = null;

    public AudioSource audioSource;
    public float volume = 0.1f;
    [HideInInspector]
    public float bpm;
    public float compensation = 400;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        bpm = audioSource.GetComponent<BeatSynchronizer>().bpm;
    }

	private void Start()
	{
        audioSource.volume = volume;
	}

	/* GET BEAT INDEX IN SEQUENCE
     * from zero to frequency(excluded)
     * this will round beat so it will start before actual beat
     * for example frequency 3 will return one of the beats 0, 1 and 2 
     * 0>0.5 = 0 | 0.6>1.5 = 1 | 1.6>2.5 = 2 | 2.5 > frequency = 0
     */
	public int GetBeatInSequenceAprox(int frequency, float beatInSamples)
    {
        int rounded = (int)Mathf.Round((audioSource.timeSamples / beatInSamples) % frequency);
        if (rounded == frequency)
            rounded = 0;

        return rounded;
    }
    public int GetBeatInSequenceAprox(int frequency, float beatInSamples, int timeSamples)
    {
        int rounded = (int)Mathf.Round((timeSamples / beatInSamples) % frequency);
        if (rounded == frequency)
            rounded = 0;

        return rounded;
    }

    /* GET CURRENT BEAT APROX
     * returns beat number in a given audio time.
     * for example  beats 0, 1 and 2 will be 
     * 0>0.5 = 0 | 0.6>1.5 =1 | 1.6>2.5 =2 | 2.5 >3= 0
     */
    public static int GetCurrentBeatAprox(int timeSamples, float beatInSamples)
    {
        return (int)Mathf.Round(timeSamples / beatInSamples);
    }

	public float GetBeatInSamples(BeatValue beatValue){
        return (60f / (bpm * BeatDecimalValues.values[(int)beatValue])) * audioSource.clip.frequency;
    }

	public float GetBeatInSeconds(){
		return 60/bpm;
	}

    public int GetAudioTimeSamples(){
        return audioSource.timeSamples;
    }




}
