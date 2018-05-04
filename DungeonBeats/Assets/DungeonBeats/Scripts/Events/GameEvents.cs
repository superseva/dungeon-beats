using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public delegate void BeatTick(int currentBeat, int beatInSequence);
    public static event BeatTick OnBeatTick;
    public static void BeatTickEvent(int currentBeat, int beatInSequence)
    {
        if (OnBeatTick != null)
            OnBeatTick(currentBeat, beatInSequence);
    }

    public delegate void SwitchCharacterToTap();
    public static event SwitchCharacterToTap OnSwitchCharacterToTap;
    public static void SwitchCharacterToTapEvent()
    {
        if (OnSwitchCharacterToTap != null)
            OnSwitchCharacterToTap();
    }

    public delegate void TapCharacterSuccess();
    public static event TapCharacterSuccess OnTapCharacterSuccess;
    public static void TapCharacterSuccessEvent()
    {
        if (OnTapCharacterSuccess != null)
            OnTapCharacterSuccess();
    }

    public delegate void TapCharacterMiss();
    public static event TapCharacterMiss OnTapCharacterMiss;
    public static void TapCharacterMissEvent()
    {
        if (OnTapCharacterMiss != null)
            OnTapCharacterMiss();
    }

    public delegate void TapMonsterSuccess();
    public static event TapMonsterSuccess OnTapMonsterSuccess;
    public static void TapMonsterSuccessEvent()
    {
        if (OnTapMonsterSuccess != null)
            OnTapMonsterSuccess();
    }

    public delegate void TapMonsterMiss();
    public static event TapMonsterMiss OnTapMonsterMiss;
    public static void TapMonsterMissEvent()
    {
        if (OnTapMonsterMiss != null)
            OnTapMonsterMiss();
    }

}