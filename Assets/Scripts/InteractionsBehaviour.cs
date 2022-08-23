using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionsBehaviour : MonoBehaviour
{
    private IInteracteable _target;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("CanCollect"))
        {
            _target = collision.GetComponent<IInteracteable>();
            _target.GetInteracted(this);
        }
        if (collision.CompareTag("Interact"))
        {
            Debug.Log("toched");
            _target = collision.GetComponent<IInteracteable>();
            _target.GetInteracted(this);
        }
    }

}
