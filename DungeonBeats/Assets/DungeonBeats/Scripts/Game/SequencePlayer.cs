using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SynchronizerData;

public class SequencePlayer : MonoBehaviour {

    public static SequencePlayer instance;

    private BeatObserverSequenced beatObserver;
    private List<int> sequence = new List<int>();
    SequenceCollection.HeroAction heroAction = SequenceCollection.HeroAction.Idle;

    private void Awake(){
        if(instance==null){
            instance = this;
        }else if(instance!=this){
            Destroy(gameObject);
        }
    }

	// Use this for initialization
	void Start () {
        beatObserver = gameObject.GetComponent<BeatObserverSequenced>();
	}
	
    // HERO ACTION CAN BE NONE IF SEQUENCDE NOT EXIST OR SOME OF THE ACTION ENUMS
    public void FilterSequence(List<int> recording)
	{
        heroAction = SequenceCollection.instance.CheckIfSequenceExsist(recording);
    }

    
    //PLAY RECORDED SEQUENCE
    // sequence last until next time is clled
    // sequence can be idle or none too
    private void PlaySequence()
    {
        RhythmEvents.PlaySequenceStartedEvent(heroAction);
    }

    // STOP WHILE RECORDING
    // not using it at the moment
	private void StopSequence(){
		RhythmEvents.PlaySequenceStartedEvent(SequenceCollection.HeroAction.Idle);
	}

	// Update is called once per frame
	void Update () {
        if((beatObserver.beatMask & BeatType.DownBeat)==BeatType.DownBeat){
            if (beatObserver.beatInSequence==0 && SequenceRecorder.instance.isRecording==false){
                PlaySequence();
			}else if(beatObserver.beatInSequence == 0 && SequenceRecorder.instance.isRecording == true){
				StopSequence();
			}
        }
	}
}
