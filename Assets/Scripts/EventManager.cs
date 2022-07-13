using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static event Action OnPeachUp;
    public static event Action OnDamaged;

    public static void SendPeachUp()
    {
        OnPeachUp?.Invoke();
    }

    public static void SendDamaged()
    {
        OnDamaged?.Invoke();
    }
}
