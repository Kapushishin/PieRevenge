using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour, IInteracteable
{
    [SerializeField] private string _textPrompt;
    [SerializeField] private int _sceneDirection;
    [SerializeField] private AudioSource _openedSound;
    [SerializeField] private GameObject _fading;
    public string PromptText => _textPrompt;

    public bool GetInteracted(InteractionsBehaviour target)
    {
        _openedSound.Play();
        _fading.SetActive(true);
        Invoke("LoadNextScene", 2f);
        return true;
    }

    private void LoadNextScene()
    {
        var index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(index + _sceneDirection);
    }
}
