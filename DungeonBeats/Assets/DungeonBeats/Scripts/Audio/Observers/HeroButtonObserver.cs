using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SynchronizerData;
using UnityEngine.UI;
using DG.Tweening;

public class HeroButtonObserver : MonoBehaviour {

    Image image;
    BeatObserver beatObserver;
    Vector2 originalSize;
    Vector2 maxSize = new Vector2(90, 90);

	void Start ()
    {
        image = gameObject.GetComponent<Image>();
        beatObserver = gameObject.GetComponent<BeatObserver>();
	}
	
	void Update ()
    {
        if((beatObserver.beatMask & BeatType.DownBeat)==BeatType.DownBeat)
        {
            image.rectTransform.sizeDelta = maxSize;
            image.rectTransform.DOSizeDelta(originalSize, 0.2f).SetEase(Ease.InCubic);
        }
	}
}
