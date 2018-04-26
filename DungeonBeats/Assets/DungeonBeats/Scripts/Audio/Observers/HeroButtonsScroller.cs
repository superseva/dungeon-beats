using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SynchronizerData;
using DG.Tweening;

public class HeroButtonsScroller : MonoBehaviour {

    public Image frame;
    public Image[] icons;
    public Vector2 activePosition = new Vector2(50,50);
    public Vector2 icoDistance = new Vector2(100,0);
    public int beatFrequency = 2;
    public int startDelay = 3;
    public int paternFrequency = 8;
    

    BeatObserver beatObserver;
    private int beatCount = 0;
    private int paternCount = 0;
    private int scrollCount = 0;
    private int currentIcoIndex = 0;
    private int nextIcoIndex;

    void Start ()
    {
        beatObserver = GetComponent<BeatObserver>();
        Reset();
	}

    private void Reset()
    {
        beatCount = 0;
        //OrganizeIcons();
    }

    private void OrganizeIcons()
    {
        for(int i =0; i< icons.Length; i++)
        {
            Image ico = icons[i] as Image;
            ico.rectTransform.anchoredPosition = (activePosition+icoDistance) + icoDistance * i;
        }
    }


    void Update ()
    {

        if ((beatObserver.beatMask & BeatType.OnBeat) == BeatType.OnBeat)
        {
            beatCount++;
            paternCount++;
            if (paternCount > paternFrequency)
            {
                paternCount = 1;
            }

            if ((beatCount - 1) % 16==0)
            {
                Debug.Log("CICLUS STARTED WITH NOTE " + beatCount);
            }

            if (beatCount % beatFrequency == 0 )
            {
                // UP BEATS
                // EVEN NUMBERS
                // use for movement

                frame.rectTransform.sizeDelta = new Vector2(100, 100);

                if (beatCount < startDelay)
                    return;

                if (beatCount % 16 == 0 && beatCount > 0)
                {
                    OrganizeIcons();
                }

                // if (scrollCount < 4)
                // {
                foreach (Image ico in icons)
                {
                        ico.rectTransform.DOAnchorPos(ico.rectTransform.anchoredPosition - icoDistance, 0.1f).SetEase(Ease.OutCubic);
                }
                 //   scrollCount++;
                //}

            }
            else
            {
                // DOWN BEAT
                // ODD NUMBERS
                // use to mark HITS
                frame.rectTransform.sizeDelta = new Vector2(150, 150);
                frame.rectTransform.DOSizeDelta(new Vector2(100, 100), 0.2f);

            }

        }


        
    }
}
