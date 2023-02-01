using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggersInit : MonoBehaviour
{
    [SerializeField] public static List<GameObject> triggers = new();

    private void Start()
    {
        GetBGTriggers();
    }

    private void GetBGTriggers()
    {
        foreach (var triggerChild in gameObject.GetComponentsInChildren<Transform>(true))
        {
            triggers.Add(triggerChild.gameObject);
            /*
            foreach (var triggerChildsChild in triggerChild.GetComponentsInChildren<Transform>(true))
            {
                triggers.Add(triggerChildsChild.gameObject);
            }
            */
        }
    }
}
