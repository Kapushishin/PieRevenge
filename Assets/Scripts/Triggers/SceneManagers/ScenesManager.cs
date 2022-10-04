using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour, IInteracteable
{
    [SerializeField] private string _textPrompt;
    [SerializeField] private int _sceneDirection;
    public string PromptText => _textPrompt;

    public bool GetInteracted(InteractionsBehaviour target)
    {
        var index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(index + _sceneDirection);
        return true;
    }
}
