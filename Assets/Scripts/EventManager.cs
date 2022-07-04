using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static event Action OnPeachUp;

    public static void SendPeachUp()
    {
        OnPeachUp?.Invoke();
    }
}
