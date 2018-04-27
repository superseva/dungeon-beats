using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SynchronizerData;
using DG.Tweening;
using System;

public class HeroButtonsScroller : MonoBehaviour {

    public Image frame;
    public Image[] icons;
    public Vector2 activePosition = new Vector2(70,70);
    public Vector2 icoDistance = new Vector2(100,0);
    public int startDelay = 3;
    //public int paternFrequency = 16;

    private BeatObserver beatObserver;
    private int beatCount = 0;
    private int paternCount = 0;
    private int repositioningCount = 0;
    public Dictionary<int, int> movementBeats = new Dictionary<int, int>();
    private Dictionary<int, int> positioningBeats = new Dictionary<int, int>();
    private Dictionary<int, int> swapCharacterBeats = new Dictionary<int, int>();
    private Image icoToReposition;
    private Vector2 icoSize = new Vector2(70, 70);

    void Start ()
    {
        
        movementBeats.Add(2, 1);
        movementBeats.Add(4, 1);
        movementBeats.Add(6, 1);
        movementBeats.Add(8, 1);
        movementBeats.Add(16, 1);

        // POSITIONING ICONS
        positioningBeats.Add(9, 1);
        positioningBeats.Add(10, 1);
        positioningBeats.Add(11, 1);
        positioningBeats.Add(12, 1);

        //SWAPING CHARACTERS ON EVENT BEATS
        swapCharacterBeats.Add(2, 1);
        swapCharacterBeats.Add(4, 1);
        swapCharacterBeats.Add(6, 1);
        swapCharacterBeats.Add(16, 1);

        beatObserver = GetComponent<BeatObserver>();
        Reset();
	}

    private void Reset()
    {
        beatCount = 0;
    }

    private void OrganizeIcons()
    {
        for(int i =0; i< icons.Length; i++)
        {
            Image ico = icons[i] as Image;
            ico.rectTransform.anchoredPosition = (activePosition+icoDistance) + icoDistance * i;
        }
    }

    // DOWN BEAT
    // ODD NUMBERS
    // use to mark HITS
    private void OnDownBeat()
    {
        //Debug.Log("TIME:" + (AudioSettings.dspTime - BeatSynchronizer.START_SONG_TIME));
        frame.rectTransform.sizeDelta = new Vector2(150, 150);
        frame.rectTransform.DOSizeDelta(new Vector2(100, 100), 0.1f);
    }

    // UP BEATS
    // EVEN NUMBERS
    // use for movement
    private void OnUpBeat()
    {
        if (movementBeats.ContainsKey(paternCount))
        {
            foreach (Image ico in icons)
            {
                ico.rectTransform.DOAnchorPos(ico.rectTransform.anchoredPosition - icoDistance, 0.1f);

            }
        }

        if(swapCharacterBeats.ContainsKey(paternCount)){
            //SWAP TO NEXT CHARACTER
            GameEvents.SwitchCharacterToTapEvent();
        }
    }

    private void OnRepositionIcon()
    {
        icoToReposition = icons[repositioningCount] as Image;
        icoToReposition.rectTransform.sizeDelta = Vector2.zero;
        icoToReposition.rectTransform.anchoredPosition = activePosition + new Vector2(icoDistance.x * (repositioningCount + 1), 0);
        icoToReposition.rectTransform.DOSizeDelta(icoSize , 0.05f);
        repositioningCount++;
        if (repositioningCount >= 4)
            repositioningCount = 0;
    }

    void Update ()
    {
        if ((beatObserver.beatMask & BeatType.DownBeat) == BeatType.DownBeat)
        {
            beatCount++;
            paternCount++;
            if (paternCount > TurnManager.BeatsInTurn)
                paternCount = 1;

            // POSITION ICONS BACK TO ORIGINAL SPOT
            if (positioningBeats.ContainsKey(paternCount))
                OnRepositionIcon();

            if (beatCount % 2 == 0 )
                OnUpBeat();
            else
                OnDownBeat();
        }
    }
}
