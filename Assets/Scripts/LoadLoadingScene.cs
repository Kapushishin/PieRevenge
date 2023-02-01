using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLoadingScene : MonoBehaviour
{
    private void Start()
    {
        SceneManager.LoadSceneAsync("LoadingScreen");
    }
}
