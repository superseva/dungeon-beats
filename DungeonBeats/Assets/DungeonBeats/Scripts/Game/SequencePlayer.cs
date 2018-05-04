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
	
    // HERO ACtioN CAN BE NONE IF SEQUENCDE NOT EXIST OR SOME OF THE ACTION ENUMS
    public void FilterSequence(List<int> recording){
        heroAction = SequenceCollection.instance.CheckIfSequenceExsist(recording);
        //Debug.Log(heroAction.ToString());
    }

    private List<int> CheckMatch(List<int> l1, List<int> l2)
    {
        if (l1.Count != l2.Count)
            return null;
        for (int i = 0; i < l1.Count; i++)
        {
            if (l1[i] != l2[i])
                return null;
        }
        return l1;
    }

    private void PlaySequence()
    {
        RhythmEvents.PlaySequenceStartedEvent(heroAction);

        if(heroAction == SequenceCollection.HeroAction.None){
            //Debug.Log("ACTION DOESN'T EXSIST....");
            return;
        }
        else if(heroAction == SequenceCollection.HeroAction.Idle){
            //Debug.Log("IDLE....");
            return;
        }
        else{
            //Debug.Log("ACTION :: " + heroAction.ToString());
            //Debug.Log("ACTION :: "+ heroAction.ToString() + "__" + SequenceCollection.instance.ALL_SEQUENCES[heroAction][beatObserver.beatInSequence]);
        }
    }
	// Update is called once per frame
	void Update () {
        if((beatObserver.beatMask & BeatType.DownBeat)==BeatType.DownBeat){
            if (beatObserver.beatInSequence==0 && SequenceRecorder.instance.isRecording==false){
                //Debug.Log("PLAY SEQUENCE");
                PlaySequence();
            }
        }
	}
}
