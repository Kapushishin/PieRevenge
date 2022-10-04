using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartBehavior : MonoBehaviour, IInteracteable
{
    public string PromptText => "";

    public bool GetInteracted(InteractionsBehaviour target)
    {
        EventManager.SendHeartUp();
        Destroy(gameObject);
        return true;
    }
}
