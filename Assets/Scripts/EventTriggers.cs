using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]

public class EventTriggers : MonoBehaviour
{
    public UnityEvent OnTrigger;

    private void Awake()
    {
        if (OnTrigger == null)
        {
            OnTrigger = new UnityEvent();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnTrigger.Invoke();
    }
}
