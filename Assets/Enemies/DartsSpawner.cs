using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartsSpawner : MonoBehaviour
{
    [SerializeField] private float cooldown;
    [SerializeField] private GameObject dart;
    [SerializeField] private Transform target;

    private void Start()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        Vector2 pos = target.position;
        Instantiate(dart, pos, Quaternion.Euler(0,0,90));
        yield return new WaitForSeconds(cooldown);
        Repeat();
    }

    private void Repeat()
    {
        StartCoroutine(Spawn());
    }
}
