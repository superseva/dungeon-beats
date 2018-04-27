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

        quarterNoteInSamples = (60f / (audioBpm * BeatDecimalValues.values[(int)BeatValue.QuarterBeat])) * audioSource.clip.frequency;
        quarterNoteInSeconds = quarterNoteInSamples / audioSource.clip.frequency;

        //Debug.Log("Q NOTE SAMPLE COUNT" + quarterNoteInSamples);
        //Debug.Log("Q NOTES IN SONG" + audioSource.clip.samples/quarterNoteInSamples);
        //Debug.Log("Q Note In Seconds:"+ quarterNoteInSeconds);
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

    }
}

