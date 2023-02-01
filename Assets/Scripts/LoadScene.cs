using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadScene : MonoBehaviour
{
    private AsyncOperation _operation;

    private void Start()
    {
        StartCoroutine(TimerCoroutine());
    }

    private IEnumerator TimerCoroutine()
    {
        yield return new WaitForSeconds(2f);
        StartCoroutine(LoadSceneCoroutine());
    }

    private IEnumerator LoadSceneCoroutine()
    {
        yield return null;

        _operation = SceneManager.LoadSceneAsync(LoadingData.sceneToLoad);
        _operation.allowSceneActivation = false;

        while (!_operation.isDone)
        {
            if (_operation.progress >= 0.9f)
            {
                _operation.allowSceneActivation = true;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
