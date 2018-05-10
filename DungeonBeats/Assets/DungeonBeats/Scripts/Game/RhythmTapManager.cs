using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using SynchronizerData;
using Lean.Touch;

public class RhythmTapManager : MonoBehaviour {

	public LayerMask layerMask = Physics.DefaultRaycastLayers;

    private int beatInSequence;
    private float beatInSamples;
    private BeatValue beatValue;
    
	void Start () {
        beatValue = BeatValue.EighthBeat;
        beatInSamples = TempoUtils.instance.GetBeatInSamples(beatValue);
        beatInSequence = 0;
        //Debug.Log(beatInSamples);
	}

    private void TapLeft(){
		NativeAudioPlayer.Instance.PlaySound(0);

        beatInSequence = TempoUtils.instance.GetBeatInSequenceAprox(8, beatInSamples);
        RhythmEvents.TapEventSequencedEvent(beatInSequence, 1);

        //Debug.Log("LEFT " + beatInSequence);
    }
    private void TapRight(){
		NativeAudioPlayer.Instance.PlaySound(1);

        beatInSequence = TempoUtils.instance.GetBeatInSequenceAprox(8, beatInSamples);
        RhythmEvents.TapEventSequencedEvent(beatInSequence, 2);

        //Debug.Log("RIGHT");
    }
    

	public void OnTapButton(LeanFinger finger){
		var results = LeanTouch.RaycastGui(finger.ScreenPosition, layerMask);

        if (results != null && results.Count > 0)
        {
			if (results[0].gameObject.CompareTag("HitBtnRight")){
				TapRight();
			}else if(results[0].gameObject.CompareTag("HitBtnLeft")){
				TapLeft();
			}
        }
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


        //if (Input.touchCount > 0)
        //{
        //    foreach (Touch touch in Input.touches)
        //    {
        //        if (touch.phase == TouchPhase.Began)
        //        {
        //            if (EventSystem.current.IsPointerOverGameObject(0) && EventSystem.current.currentSelectedGameObject != null)
        //            {
        //                if (EventSystem.current.currentSelectedGameObject.CompareTag("HitBtnLeft"))
        //                {
        //                    TapLeft();
        //                }else if(EventSystem.current.currentSelectedGameObject.CompareTag("HitBtnRight")){
        //                    TapRight();
        //                }
        //            }
        //        }
        //    }
        //}
	}
}
