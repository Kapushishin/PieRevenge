using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour, IInteracteable
{
    [SerializeField] private string _textPrompt;
    [SerializeField] private AudioSource _openedSound;
    [SerializeField] private GameObject _fading;
    public string PromptText => _textPrompt;

    public bool GetInteracted(InteractionsBehaviour target)
    {
        _openedSound.Play();
        _fading.SetActive(true);
        StartCoroutine(LoadSceneCoroutine());
        return true;
    }

    private IEnumerator LoadSceneCoroutine()
    { 
        yield return new WaitForSeconds(1.5f);
        GetComponent<SceneLoadTrigger>().LoadNewScene();
    }
}
