using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static event Action OnPeachUp;
    public static event Action OnNPCInteract;

    //public delegate void OnNextLine();
    //public static event OnNextLine onNextLine;
    public static event Action OnNextLine;


    public static void SendPeachUp()
    {
        OnPeachUp?.Invoke();
    }

    public static void SendNPCInteract()
    {
        OnNPCInteract?.Invoke();
    }

    public static void SendNextLine()
    {
        OnNextLine?.Invoke();
    }
}
