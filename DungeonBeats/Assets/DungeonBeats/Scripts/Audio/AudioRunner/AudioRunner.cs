using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SynchronizerData;
using System;
using DG.Tweening;

public class AudioRunner : MonoBehaviour {


public BeatValue beatValue = BeatValue.QuarterBeat;
public AudioSource audioSource;
public float fullNoteWidth = 1;
public float fullNoteInSamples;
public float fullNoteInSecs;

private float quarterNoteInSamples;
private float quarterNoteInSeconds;
private float samplePercentage;
private float moveAmount;

private float songCurrentTime;
private float songPercent;

private float prevSamples = 0;
private float sampleDif = 0;

private BeatObserver beatObserver;

private Tween tween;


// Use this for initialization
void Start () {
        
        beatObserver = GetComponent<BeatObserver>();

        float audioBpm = audioSource.GetComponent<BeatSynchronizer>().bpm;
        fullNoteInSamples = (60f / (audioBpm * BeatDecimalValues.values[(int)BeatValue.WholeBeat])) * audioSource.clip.frequency;
        fullNoteInSecs = fullNoteInSamples / audioSource.clip.frequency;


        quarterNoteInSamples = (60f / (audioBpm * BeatDecimalValues.values[(int)beatValue])) * audioSource.clip.frequency;
        quarterNoteInSeconds = quarterNoteInSamples / audioSource.clip.frequency;


        Debug.Log("FULL NOTE SAMPLE COUNT" + fullNoteInSamples);
        Debug.Log("FULL NOTES IN SONG" + audioSource.clip.samples/fullNoteInSamples);
        Debug.Log("FULL Note In Seconds:"+ fullNoteInSecs);
        
    }

    // Update is called once per frame
    void Update()
    {

        // RunPlayHead();
        if ((beatObserver.beatMask & BeatType.DownBeat) == BeatType.DownBeat)
        {
            //anim.SetTrigger("DownBeatTrigger");
            //transform.localScale = new Vector3(2, 2, 2);
           // Debug.Log("DOWN");
            RunPlayHead();
        }
        if ((beatObserver.beatMask & BeatType.UpBeat) == BeatType.UpBeat)
        {
            //anim.SetTrigger("DownBeatTrigger");
            //transform.localScale = new Vector3(2, 2, 2);
           // Debug.Log("UP");
           
        }



    }

    float wholeNotesCount = 0;

    void RunPlayHead()
    {
        
       
            //anim.SetTrigger("DownBeatTrigger");
            if (tween != null)
            {
                if (tween.IsPlaying())
                {
                    tween.Complete();
                }
            }

        tween = transform.DOMoveX(transform.position.x + fullNoteWidth, fullNoteInSecs). SetEase(Ease.Linear);//SetEase(Ease.InOutSine);
                                                                                                              //.SetEase(Ease.Linear);
        wholeNotesCount++;

        

        //Debug.Log(audioSource.timeSamples);

        // transform.position += new Vector3(1 * Time.deltaTime, 0, 0);

        //sampleDif = audioSource.timeSamples - prevSamples;
        //samplePercentage = sampleDif / fullNoteInSamples;
        //moveAmount = fullNoteWidth * samplePercentage;
        //moveAmount = (float) Math.Round((double)moveAmount, 2);
        //Debug.Log(sampleDif +"..."+ moveAmount);
        //transform.position += new Vector3(moveAmount*Time.deltaTime,0,0);

        //prevSamples = audioSource.timeSamples;

        //songCurrentTime = (float)audioSource.timeSamples / AudioSettings.outputSampleRate;
        //songPercent = songCurrentTime / audioSource.clip.length;
        //quarterModulo = audioSource.timeSamples % quarterNoteInSamples;
        //quarterSamplePercent = quarterModulo / quarterNoteInSamples;
        //quarterPixelPercent = quarterSamplePercent * quarterNoteInPixels;

        //transform.Translate(new Vector3(1,0,0) * Time.deltaTime);

    }
}

