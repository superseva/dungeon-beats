using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using SynchronizerData;

public class HeroTapManager : MonoBehaviour
{
    //public AudioSource audioSource;
    public float samplesMissTreshold = 2500;
    private Dictionary<int, int> tapBeats = new Dictionary<int, int>();
    private int beatInSequence;
    private int currentBeat;

    void Start()
    {
        tapBeats.Add(0, 1);
        tapBeats.Add(4, 1);
        tapBeats.Add(8, 1);
        tapBeats.Add(12, 1);
    }

    public void HeroButtonOnTap()
    {
        beatInSequence = TurnManager.GetBeatInSequenceAprox(TurnManager.BeatsInTurn, TempoUtils.instance.audioSource.timeSamples, TurnManager.instance.beatSamples);
        if (tapBeats.ContainsKey(beatInSequence))
        {
            currentBeat = TurnManager.GetCurrentBeatAprox(TempoUtils.instance.audioSource.timeSamples, TurnManager.instance.beatSamples);
            float diff = (currentBeat * TurnManager.instance.beatSamples) - (TempoUtils.instance.audioSource.timeSamples-TempoUtils.instance.compensation);
            //Debug.Log(beatInSequence + "" + diff);
            if (Mathf.Abs(diff) < samplesMissTreshold)
            {
                // HIT ON TIME
                GameEvents.TapCharacterSuccessEvent();
            }
            else
            {
                // HIT OUT OF TIME BOUNDS
                GameEvents.TapCharacterMissEvent();
            }
        }
    }

    void Update()
    {

        #if UNITY_EDITOR
            
        if(Input.GetKeyDown(KeyCode.Space)){
            HeroButtonOnTap();
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
                        if (EventSystem.current.currentSelectedGameObject.CompareTag("HitBtn"))
                        {
                            HeroButtonOnTap();
                        }
                    }
                }
            }
        }
    }
}
