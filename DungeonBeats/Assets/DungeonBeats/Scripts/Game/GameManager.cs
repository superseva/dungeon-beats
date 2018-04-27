using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SynchronizerData;

public class GameManager : MonoBehaviour {
    
    public static GameManager instance = null;
    public enum CharacterType{
        Fighter,
        Rogue,
        Wizard,
        Cleric,
        None
    }
    public enum CharacterAction{
        Attack,
        Block,
        Heal,
        Idle,
        Wait
    }

    public CharacterType[] characters;

    private int characterIndex;
    private bool isCharacterClicked = false;
    private Dictionary<CharacterType, CharacterAction> recordedActions = new Dictionary<CharacterType, CharacterAction>();
    private BeatObserver beatObserver;
    private int beatCount;

	private void Awake()
	{
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);    
	}

	private void Start()
	{
        beatObserver = gameObject.GetComponent<BeatObserver>();
        beatCount = 0;
        recordedActions.Add(CharacterType.Fighter, CharacterAction.Idle);
        recordedActions.Add(CharacterType.Rogue, CharacterAction.Idle);
        recordedActions.Add(CharacterType.Wizard, CharacterAction.Idle);
        recordedActions.Add(CharacterType.Cleric, CharacterAction.Idle);
	}

	private void OnEnable()
	{
        GameEvents.OnSwitchCharacterToTap += SwitchCharacterToTap;
        GameEvents.OnTapCharacterSuccess += OnTapCharacterSuccess;
        GameEvents.OnTapCharacterMiss += OnTapCharacterMiss;
	}

	private void OnDisable()
	{
        GameEvents.OnSwitchCharacterToTap -= SwitchCharacterToTap;
        GameEvents.OnTapCharacterSuccess -= OnTapCharacterSuccess;
        GameEvents.OnTapCharacterMiss -= OnTapCharacterMiss;
	}

    private void ResetCharacterActions()
    {
        recordedActions[CharacterType.Fighter] = CharacterAction.Idle;
        recordedActions[CharacterType.Rogue] = CharacterAction.Idle;
        recordedActions[CharacterType.Wizard] = CharacterAction.Idle;
        recordedActions[CharacterType.Cleric] = CharacterAction.Idle;
    }

    /*
     * We fire this event from HeroButtonScroller
     */
    private void SwitchCharacterToTap()
    {
        if (characterIndex < characters.Length - 1)
        {
            characterIndex++;
        }
        else
        {
            characterIndex = 0;
            ResetCharacterActions();
        }
        isCharacterClicked = false;
    }

    public CharacterType GetCharacterForTap(){
        return characters[characterIndex];
    }

    /* 
     * We fire the event from the TapHeroManager
     */
    private void OnTapCharacterSuccess()
    {
        if (isCharacterClicked)
            return;
        recordedActions[GetCharacterForTap()] = CharacterAction.Wait;
        isCharacterClicked = true;
    }
    /* 
     * We fire the event from the TapHeroManager
     */
    private void OnTapCharacterMiss()
    {
        //Debug.Log("TAP MISS >>> " + GetCharacterForTap().ToString());
        if (isCharacterClicked)
            return;
        recordedActions[GetCharacterForTap()] = CharacterAction.Idle;
        isCharacterClicked = true;
    }
    /* 
     * We fire the event from the TapMonsterManager
     */
    private void OnTapMonsterSuccess()
    {
        if(recordedActions[GetCharacterForTap()]==CharacterAction.Wait){
            recordedActions[GetCharacterForTap()] = CharacterAction.Attack;
        }else{
            recordedActions[GetCharacterForTap()] = CharacterAction.Idle;
        }
    }

	private void Update()
	{
        if((beatObserver.beatMask & BeatType.DownBeat) == BeatType.DownBeat)
        {
            beatCount++;
            if(beatCount%2==0){
                // DO RECORDED ACTIONS
                //Debug.Log("------------------RECORDED ACtioNS");
                string str = "";
                foreach(KeyValuePair<CharacterType,CharacterAction> entry in recordedActions){
                    str += entry.Key.ToString() + " DOING " + entry.Value.ToString() + " //// ";
                }
                Debug.Log(str);
            }
        }
	}
}
