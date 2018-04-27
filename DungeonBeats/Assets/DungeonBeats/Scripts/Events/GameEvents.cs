using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour 
{
    public delegate void SwitchCharacterToTap();
    public static event SwitchCharacterToTap OnSwitchCharacterToTap;
    public static void SwitchCharacterToTapEvent(){
        if (OnSwitchCharacterToTap != null)
            OnSwitchCharacterToTap();
    }

    public delegate void TapCharacterSuccess();
    public static event TapCharacterSuccess OnTapCharacterSuccess;
    public static void TapCharacterSuccessEvent(){
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
}
