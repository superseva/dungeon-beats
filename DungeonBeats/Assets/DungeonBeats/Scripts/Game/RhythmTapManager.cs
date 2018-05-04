using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using SynchronizerData;

public class RhythmTapManager : MonoBehaviour {

    private int beatInSequence;
    private float beatInSamples;
    private BeatValue beatValue;
    //private 

	void Start () {
        beatValue = BeatValue.EighthBeat;
        beatInSamples = TempoUtils.instance.GetBeatInSamples(beatValue);
        beatInSequence = 0;
        //Debug.Log(beatInSamples);
	}

    private void TapLeft(){
        beatInSequence = TempoUtils.instance.GetBeatInSequenceAprox(8, beatInSamples);
        RhythmEvents.TapEventSequencedEvent(beatInSequence, 1);
        //Debug.Log("LEFT " + beatInSequence);
    }
    private void TapRight(){
        beatInSequence = TempoUtils.instance.GetBeatInSequenceAprox(8, beatInSamples);
        RhythmEvents.TapEventSequencedEvent(beatInSequence, 2);
        //Debug.Log("RIGHT");
    }

	// Update is called once per frame
	void Update () {
        #if UNITY_EDITOR

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            TapLeft();
        }else if(Input.GetKeyDown(KeyCode.RightArrow)){
            TapRight();
        }

        #endif


        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    if (EventSystem.current.IsPointerOverGameObject(0) && EventSystem.current.currentSelectedGameObject != null)
                    {
                        if (EventSystem.current.currentSelectedGameObject.CompareTag("HitBtnLeft"))
                        {
                            TapLeft();
                        }else if(EventSystem.current.currentSelectedGameObject.CompareTag("HitBtnRight")){
                            TapRight();
                        }
                    }
                }
            }
        }
	}
}
