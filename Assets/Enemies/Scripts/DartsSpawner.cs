using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartsSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _target;
    [SerializeField] private float _frequency = 3f;
    [SerializeField] private GameObject[] darts;

    private void Start()
    {
        StartCoroutine(SpawnCoroutine());
    }

    private void ShootDart()
    {
        GameObject dart = ObjectPool.Instance.GetPooledObject();

        if (dart != null)
        {
            dart.transform.position = _target.transform.position;
            dart.transform.rotation = _target.transform.rotation;
            dart.SetActive(true);
        }
    }

    private int FindDart()
    {
        for (int i = 0; i < darts.Length; i++)
        {
            if (!darts[i].activeInHierarchy)
            {
                return i;
            }
        }
        return 0;
    }

    private void SpawnDart()
    {
        darts[FindDart()].transform.position = _target.transform.position;
        darts[FindDart()].transform.rotation = _target.transform.rotation;
        darts[FindDart()].GetComponent<DartTrapBehavior>().ActivateDart();
    }

    private IEnumerator SpawnCoroutine()
    {
        //ShootDart();
        SpawnDart();
        yield return new WaitForSeconds(_frequency);
        RepeatSpawn();
    }

    private void RepeatSpawn()
    {
        StartCoroutine(SpawnCoroutine());
    }
}
