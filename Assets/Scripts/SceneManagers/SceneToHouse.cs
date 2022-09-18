using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneToHouse : MonoBehaviour, IInteracteable
{
    private string _promptText;
    [SerializeField] private string _promptTextClosed;
    [SerializeField] private string _promptTextOpened;
    [SerializeField] private AudioSource _doorOpenedSound;
    public string PromptText => TextChoose();

    public bool GetInteracted(InteractionsBehaviour target)
    {
        if (SwitchParametres.CanGoHome == true)
        {
            LoadScene();
        }
        else
        {
            _promptText = _promptTextClosed;
        }
        return true;
    }

    private void LoadScene()
    {
        SceneManager.LoadScene("House");
    }

    private string TextChoose()
    {
        if (SwitchParametres.CanGoHome == true)
        {
            return _promptTextOpened;
        }
        else
        {
            return _promptTextClosed;
        }
    }
}
