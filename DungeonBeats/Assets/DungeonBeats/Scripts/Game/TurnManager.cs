using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SynchronizerData;

public class TurnManager : MonoBehaviour
{
    public static TurnManager instance = null;

    public static int BeatsInTurn = 32;
    public BeatValue beatValue = BeatValue.QuarterBeat;
    public float beatSamples;
    [HideInInspector]
    public int currentBeat = 0;
    public int beatInSequence = 0;

    private BeatObserverSequenced beatObserver;
    private Dictionary<int, System.Action> actions = new Dictionary<int, System.Action>();

	private void Awake()
	{
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

       
	}

    /* GET BEAT INDEX IN SEQUENCE
     * from zero to frequency(excluded)
     * this will round beat so it will start before actual beat
     * for example frequency 3 will return one of the beats 0, 1 and 2 
     * 0>0.5 = 0 | 0.6>1.5 = 1 | 1.6>2.5 = 2 | 2.5 > frequency = 0
     */
    public static int GetBeatInSequenceAprox(int frequency, int timeSamples, float beatInSamples)
    {
        int rounded = (int)Mathf.Round((timeSamples / beatInSamples) % frequency);
        if (rounded == frequency)
            rounded = 0;

        return rounded;
    }

    /* GET CURRENT BEAT APROX
     * returns beat number in a given audio time.
     * for example  beats 0, 1 and 2 will be 
     * 0>0.5 = 0 | 0.6>1.5 =1 | 1.6>2.5 =2 | 2.5 >3= 0
     */
    public static int GetCurrentBeatAprox(int timeSamples, float beatInSamples)
    {
        return (int)Mathf.Round(timeSamples / beatInSamples);
    }

	private void Start()
	{
        beatObserver = gameObject.GetComponent<BeatObserverSequenced>();
        currentBeat = 0;

        actions[16] = HeroAction;
        actions[18] = HeroAction;
        actions[20] = HeroAction;
        actions[22] = HeroAction;

        actions[24] = NpcAction;
        actions[26] = NpcAction;
        actions[28] = NpcAction;
        actions[30] = NpcAction;

        beatSamples = TempoUtils.instance.GetBeatInSamples(beatValue);

        Debug.Log("SAMPLES IN BEAT " + beatSamples);
	}

    private void HeroAction(){
        //Debug.Log("HERO ACTION");
    }

    private void NpcAction(){
        //Debug.Log("NPC ACTION");
    }

    System.Action outVal;
	private void Update()
	{
        if((beatObserver.beatMask & BeatType.DownBeat)==BeatType.DownBeat){
            //beatInSequence = currentBeat % BeatsInTurn;
            currentBeat = beatObserver.currentBeat;
            beatInSequence = beatObserver.beatInSequence;
            if(actions.TryGetValue(beatObserver.beatInSequence, out outVal)){
                actions[beatObserver.beatInSequence]();
            }

            //currentBeat++;
        }
	}

}
