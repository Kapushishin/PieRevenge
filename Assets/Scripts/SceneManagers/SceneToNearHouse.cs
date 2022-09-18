using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneToNearHouse : MonoBehaviour, IInteracteable
{
    [SerializeField] private string _promptText;
    [SerializeField] private AudioSource _doorOpenedSound;
    public string PromptText => _promptText;

    public bool GetInteracted(InteractionsBehaviour target)
    {
        LoadScene();
        return true;
    }

    private void LoadScene()
    {
        SceneManager.LoadScene("Near House");
    }
}
