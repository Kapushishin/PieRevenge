using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBehavior : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField, Range(0f, 1f)] private float str;
    [SerializeField] private bool verticalParallax;
    private Vector3 prevPosition;

    private void Start()
    {
        if (!target)
            target = Camera.main.transform;

        prevPosition = target.position;
    }

    private void Update()
    {
        Vector3 delta = target.position - prevPosition;

        if (!verticalParallax)
            delta.y = 0;

        prevPosition = target.position;

        transform.position += delta * str;
    }
}
