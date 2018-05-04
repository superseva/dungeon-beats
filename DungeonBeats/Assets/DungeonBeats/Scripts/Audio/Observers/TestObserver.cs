using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SynchronizerData;

public class TestObserver : MonoBehaviour {


    private BeatObserver beatObserver;

	private void Start()
	{
        beatObserver = GetComponent<BeatObserver>();
	}

    int counter = 0;
	private void Update()
	{
        if((beatObserver.beatMask & BeatType.OnBeat)==BeatType.OnBeat){
            counter++;
            Debug.Log(counter);
        }
	}

	/*
	private void OnEnable()
	{
        GameEvents.OnBeatTick += OnTick;
	}
	private void OnDisable()
	{
        GameEvents.OnBeatTick -= OnTick;
	}

    private void OnTick(int currentBeat, int beatInSqnc){
        Debug.Log(currentBeat + "---" + beatInSqnc);
    }
    */
}
