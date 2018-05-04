using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SynchronizerData;
using DG.Tweening;

public class Monster : MonoBehaviour, IClickableOnBeat, IKillable, IDamageable {
    
    public Dictionary<int, int> hitBeats;
    public float samplesMissTreshold = 2500;
    public BeatValue beatValue = BeatValue.EighthBeat;
    public Transform heart;
    public Transform marker;

    public Transform[] hearts;
    public int nextHeart;
    //private List<int> beatAnims = new List<int> { 1, 0, 0, 0, 1, 1, 0, 0, 1, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    private List<int> beatAnims = new List<int> { 0, 0, 1, 0, 0, 0, 1, 1, 0, 0, 1, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

    private Tween tween;
    
    private BeatObserverSequenced beatObserver;
    private int nextBeatIndex = -1;
    private int beatInSequence;
    private int currentBeat;

	// Use this for initialization
	void Start () {
        hitBeats = new Dictionary<int, int>();
        for (int i = 0; i < beatAnims.Count;i++){
            if(beatAnims[i]==1)
                hitBeats.Add(i, 1);
        }
        //hitBeats.Add(2, 1);
        //hitBeats.Add(6, 1);
        //hitBeats.Add(10, 1);
        //hitBeats.Add(14, 1);

        beatObserver = gameObject.GetComponent<BeatObserverSequenced>();
        nextBeatIndex = -1;
	}

    int outVal;
    public void ClickOnBeat(){
        //int audioSampleTime = TempoUtils.instance.audioSource.timeSamples;
        //beatSamples = TempoUtils.instance.GetBeatInSamples(beatValue);
        beatInSequence = TurnManager.GetBeatInSequenceAprox(TurnManager.BeatsInTurn, TempoUtils.instance.audioSource.timeSamples, TurnManager.instance.beatSamples);

        if(hitBeats.TryGetValue(beatInSequence, out outVal)){
            //Debug.Log(beatInSequence);
            //Debug.Log(currentBt);
            currentBeat = TurnManager.GetCurrentBeatAprox(TempoUtils.instance.audioSource.timeSamples, TurnManager.instance.beatSamples);
            float diff = (currentBeat * TurnManager.instance.beatSamples) - (TempoUtils.instance.audioSource.timeSamples-TempoUtils.instance.compensation);
            //Debug.Log("diff"+diff);
            if (Mathf.Abs(diff) < samplesMissTreshold)
            {
                // HIT ON TIME
                GameEvents.TapMonsterSuccessEvent();
            }
            else
            {
                // HIT OUT OF TIME BOUNDS
                GameEvents.TapMonsterMissEvent();
            }
        }
    }

    public void Damage(){
        
    }

    public void Kill()
    {

    }

    //private void OnBeatAnim()
    //{
    //    heart = hearts[nextHeart];
    //    heart.position = new Vector3(0,2,0);
    //    heart.DOMoveY(0, 0.25f);
    //    nextHeart++;
    //    if (nextHeart > hearts.Length - 1)
    //        nextHeart = 0;
    //}

    private void OnBeatAnim()
    {
        heart = hearts[nextHeart];
        marker.position = heart.position + new Vector3(0, 0, -0.02f);
        SpriteRenderer sprt = heart.gameObject.GetComponent<SpriteRenderer>();
        sprt.color = clr;
        sprt.DOColor(Color.white, 0.1f).SetDelay(0.1f);
        nextHeart++;
        if (nextHeart > hearts.Length - 1)
            nextHeart = 0;
    }

    //private void OnBeatAnim()
    //{
    //    heart = hearts[nextHeart];
    //    SpriteRenderer sprt = heart.gameObject.GetComponent<SpriteRenderer>();
    //    sprt.color = Color.red;
    //    sprt.DOColor(Color.white, 0.1f).SetDelay(0.1f);
    //    nextHeart++;
    //    if (nextHeart > hearts.Length - 1)
    //        nextHeart = 0;
    //}
    //private void OnBeatAnim()
    //{
    //    if (tween != null)
    //        tween.Complete();

    //    heart.localScale = new Vector3(3, 3, 3);
    //    tween = heart.DOScale(Vector3.one, 0.1f).SetEase(Ease.Linear);
    //}

    // Update is called once per frame
    Color clr;
	void Update () {
        if((beatObserver.beatMask & BeatType.DownBeat)==BeatType.DownBeat){
            nextBeatIndex++;
            int b = nextBeatIndex % 8;
            clr = Color.white;
            if(beatAnims[b]==1){
                clr = Color.red;
                //OnBeatAnim();
            }
            OnBeatAnim();

        }
	}
}
