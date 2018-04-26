using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SynchronizerData;

public class MyObserver : MonoBehaviour {

    private BeatObserver beatObserver;
    // Use this for initialization
    void Start () {
        beatObserver = GetComponent<BeatObserver>();
    }
	
	// Update is called once per frame
	void Update () {
        if ((beatObserver.beatMask & BeatType.DownBeat) == BeatType.DownBeat)
        {

            //Debug.Log("KLIK");
        }
        if ((beatObserver.beatMask & BeatType.UpBeat) == BeatType.UpBeat)
        {
           // Debug.Log("KLOK");
        }
    }
}
