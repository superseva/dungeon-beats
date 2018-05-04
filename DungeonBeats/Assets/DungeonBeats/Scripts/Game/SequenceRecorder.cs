using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SynchronizerData;

[RequireComponent(typeof(BeatObserverSequenced))]
public class SequenceRecorder : MonoBehaviour {

    public static SequenceRecorder instance;
    public List<int> recording;
    public bool isRecording = false;

    private BeatObserverSequenced beatObserver;

	private void Awake()
	{
        if (instance == null)
            instance = this;
        else if(instance!=this)
            Destroy(gameObject);
	}

	void Start () {
        beatObserver = gameObject.GetComponent<BeatObserverSequenced>();
        ClearRecording();
	}
	
    private void OnEnable()
    {
        RhythmEvents.OnTapEventSequenced += RecordBeat;
    }

    private void OnDisable()
    {
        RhythmEvents.OnTapEventSequenced -= RecordBeat;
    }

    private void ClearRecording()
    {
        for (int i = 0; i < recording.Count; i++)
        {
            recording[i] = 0;
        }
    }

    private void RecordBeat(int beatInSequence, int finger)
    {
        if (isRecording && recording[beatInSequence] == 0)
        {
            recording[beatInSequence] = finger;
            RhythmEvents.BeatRecordedEvent(beatInSequence, finger);
        }
    }

    // This is trigered 1/16th note before full bar
	void Update () {
        if ((beatObserver.beatMask & BeatType.OffBeat) == BeatType.OffBeat)
        {
            isRecording = !isRecording;
            if (!isRecording)
            {
                // RECORDING COMPLETED
                SequencePlayer.instance.FilterSequence(recording);
                RhythmEvents.RecordingCompletedEvent();
            }
            else
            {
                // RECORDING STARTED
                ClearRecording();
                RhythmEvents.RecordingStartedEvent();
            }

        }
	}
}
