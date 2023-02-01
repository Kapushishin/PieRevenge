using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadTrigger : MonoBehaviour
{
    [SerializeField] private string targetScene;

    public void LoadNewScene()
    {
        LoadingData.sceneToLoad = targetScene;

        SwitchParametres.playerPosition = null;

        GameObject.FindGameObjectWithTag("Save System").GetComponent<SaveLoadSystem>().SaveGame();

        SceneManager.LoadScene("LoadingScreen");
    }
}
