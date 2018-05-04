using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmEvents : MonoBehaviour {

    public delegate void TapEventSequenced(int beatInSequence, int tapFinger);
    public static event TapEventSequenced OnTapEventSequenced;
    public static void TapEventSequencedEvent(int beatInSequence, int tapFinger)
    {
        if (OnTapEventSequenced != null)
            OnTapEventSequenced(beatInSequence, tapFinger);
    }

    // RECORDING EVENTS
    public delegate void RecordingStarted();
    public static event RecordingStarted OnRecordingStarted;
    public static void RecordingStartedEvent()
    {
        if (OnRecordingStarted != null)
            OnRecordingStarted();
    }

    public delegate void RecordingCompleted();
    public static event RecordingCompleted OnRecordingCompleted;
    public static void RecordingCompletedEvent()
    {
        if (OnRecordingCompleted != null)
            OnRecordingCompleted();
    }

    public delegate void BeatRecorded(int beatInSequence, int finger);
    public static event BeatRecorded OnBeatRecorded;
    public static void BeatRecordedEvent(int beatInSequence, int finger)
    {
        if (OnBeatRecorded != null)
            OnBeatRecorded(beatInSequence, finger);
    }

    // PLAYING EVENTS
    public delegate void PlaySequenceStarted(SequenceCollection.HeroAction heroAction);
    public static event PlaySequenceStarted OnPlaySequenceStarted;
    public static void PlaySequenceStartedEvent(SequenceCollection.HeroAction heroAction){
        if (OnPlaySequenceStarted != null)
            OnPlaySequenceStarted(heroAction);
    }
}
