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
    public int startDelay = 3;
    public int paternFrequency = 16;

    private BeatObserver beatObserver;
    private int beatCount = 0;
    private int paternCount = 0;
    private int sponCount = 0;
    private List<int> movementBeats = new List<int> { 2, 4, 6, 8, 16 };
    private List<int> sponBeats = new List<int> { 9, 10, 11, 12};
    private Image icoToSpon;
    private Vector2 icoSize = new Vector2(70, 70);

    void Start ()
    {
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
        frame.rectTransform.sizeDelta = new Vector2(150, 150);
        frame.rectTransform.DOSizeDelta(new Vector2(100, 100), 0.1f);
    }

    // UP BEATS
    // EVEN NUMBERS
    // use for movement
    private void OnUpBeat()
    {
        if (movementBeats.Contains(paternCount))
        {
            foreach (Image ico in icons)
            {
                ico.rectTransform.DOAnchorPos(ico.rectTransform.anchoredPosition - icoDistance, 0.1f)
                    .SetDelay(0.05f)
                    .SetEase(Ease.OutCubic);
            }
        }
    }

    private void OnSponIcon()
    {
        icoToSpon = icons[sponCount] as Image;
        icoToSpon.rectTransform.sizeDelta = Vector2.zero;
        icoToSpon.rectTransform.anchoredPosition = activePosition + new Vector2(icoDistance.x * (sponCount + 1), 0);
        icoToSpon.rectTransform.DOSizeDelta(icoSize , 0.05f);
        sponCount++;
        if (sponCount >= 4)
            sponCount = 0;
    }

    void Update ()
    {
        if ((beatObserver.beatMask & BeatType.OnBeat) == BeatType.OnBeat)
        {
            beatCount++;
            paternCount++;
            if (paternCount > paternFrequency)
                paternCount = 1;

            // SPONING ICONS BACK
            if (sponBeats.Contains(paternCount))
                OnSponIcon();

            if (beatCount % 2 == 0 )
                OnUpBeat();
            else
                OnDownBeat();
        }
    }
}
