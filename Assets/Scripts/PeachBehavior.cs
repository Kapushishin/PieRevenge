using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeachBehavior : MonoBehaviour, IInteracteable
{
    public bool GetInteracted(InteractionsBehaviour target)
    {
        EventManager.SendPeachUp();
        Destroy(gameObject);
        return true;
    }
}
