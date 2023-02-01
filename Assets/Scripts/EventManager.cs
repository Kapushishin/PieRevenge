using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static event Action OnPeachUp;
    public static event Action OnEndDialog;
    public static event Action OnHeartUp;
    public static event Action OnCanMove;
    public static event Action OnCantMove;

    public static void SendPeachUp()
    {
        OnPeachUp?.Invoke();
    }

    public static void SendDestroyNPC()
    {
        OnEndDialog?.Invoke();
    }

    public static void SendHeartUp()
    {
        OnHeartUp?.Invoke();
    }

    public static void SendCanMove()
    {
        OnCanMove?.Invoke();
    }

    public static void SendCantMove()
    {
        OnCantMove?.Invoke();
    }
}
