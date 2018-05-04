using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceCollection:MonoBehaviour {
    public static SequenceCollection instance;

    public enum HeroAction{ MoveForward, MoveBackwards, Attack, Defend, Heal, Buff, Idle, None };
    
    public Dictionary<HeroAction, List<int>> ALL_SEQUENCES = new Dictionary<HeroAction, List<int>>();

    public List<int> idle = new List<int>();
    public List<int> moveForward = new List<int>();
    public List<int> moveBackwards = new List<int>();
    public List<int> attack = new List<int>();
    public List<int> defend = new List<int>();
    public List<int> heal = new List<int>();
    public List<int> buff = new List<int>();

	private void Awake()
	{
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
	}

	void Start(){
        ALL_SEQUENCES[HeroAction.Idle] = idle;
        ALL_SEQUENCES[HeroAction.MoveForward] = moveForward;
        ALL_SEQUENCES[HeroAction.MoveBackwards] = moveBackwards;
        ALL_SEQUENCES[HeroAction.Attack] = attack;
        ALL_SEQUENCES[HeroAction.Defend] = defend;
        ALL_SEQUENCES[HeroAction.Heal] = heal;
        ALL_SEQUENCES[HeroAction.Buff] = buff;
    }

    List<int> searchedSequence;
    public HeroAction CheckIfSequenceExsist(List<int> _sequence)
    {
        foreach (KeyValuePair<SequenceCollection.HeroAction, List<int>> entry in ALL_SEQUENCES)
        {
            searchedSequence = CheckMatch(entry.Value, _sequence);
            if (searchedSequence != null)
            {
                return entry.Key;
            }
        }
        return HeroAction.None;
    }

    private List<int> CheckMatch(List<int> l1, List<int> l2)
    {
        if (l1.Count != l2.Count)
            return null;
        for (int i = 0; i < l1.Count; i++)
        {
            if (l1[i] != l2[i])
                return null;
        }
        return l1;
    }

}
