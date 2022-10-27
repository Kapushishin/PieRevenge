using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToMainMenu : MonoBehaviour
{
    private void Start()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
