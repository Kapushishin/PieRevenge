using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            GetComponent<SaveLoadSystem>().SaveGame();
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            StartCoroutine(Cooldown());
        }
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<SaveLoadSystem>().LoadGame();
    }
}
