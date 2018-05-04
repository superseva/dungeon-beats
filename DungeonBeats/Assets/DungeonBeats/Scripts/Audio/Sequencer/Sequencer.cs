using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequencer : MonoBehaviour {


    public float bpm = 120;

    public AudioSource mainRhythm;
    public AudioSource snare;
    public AudioSource hihat;

    public double delayStart = 2.0f;
    public double barLength = 2.0f;

    private double startTime;

	// Use this for initialization
	void Start () {
        startTime = AudioSettings.dspTime;
        timeCounter = startTime;


        hihat.PlayScheduled(startTime + delayStart);
        mainRhythm.PlayScheduled(startTime + 4);

        snare.PlayScheduled(startTime + 4);
        snare.SetScheduledEndTime(startTime + 8);

        //snare.PlayScheduled(startTime + 16);
        //snare.SetScheduledEndTime(startTime + 24);
	}


    double timeCounter = 0;
    // Update is called once per frame
    bool snareIsReady = false;
    void Update()
    {
        //Debug.Log(AudioSettings.dspTime);
        if (AudioSettings.dspTime == timeCounter)
        {
            timeCounter += Time.unscaledDeltaTime;
        }
        else
        {
            timeCounter = AudioSettings.dspTime;
        }

        if (timeCounter >= startTime + 12 && !snareIsReady)
        {
            snare.PlayScheduled(startTime + 16);
            snareIsReady = true;
        }
    }
           
}
