using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using SynchronizerData;

public class HeroTapManager : MonoBehaviour
{
    public AudioSource audioSource;
    public float samplesMissTreshold = 2500;
    private float beatSamples;
    private Dictionary<int, int> tapBeats = new Dictionary<int, int>();
    private int beatInSequence;
    private int currentBeat;

    void Start()
    {
        float audioBpm = audioSource.GetComponent<BeatSynchronizer>().bpm;
        beatSamples = (60f / (audioBpm * BeatDecimalValues.values[(int)BeatValue.QuarterBeat])) * audioSource.clip.frequency;
        tapBeats.Add(0, 1);
        tapBeats.Add(1, 1);
        tapBeats.Add(2, 1);
        tapBeats.Add(3, 1);
    }

    public void HeroButtonOnTap()
    {
        beatInSequence = TurnManager.GetBeatInSequenceAprox(TurnManager.BeatsInTurn, audioSource.timeSamples, beatSamples);
        if (tapBeats.ContainsKey(beatInSequence))
        {
            currentBeat = TurnManager.GetCurrentBeatAprox(audioSource.timeSamples, beatSamples);
            float diff = (currentBeat * beatSamples) - audioSource.timeSamples;
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
