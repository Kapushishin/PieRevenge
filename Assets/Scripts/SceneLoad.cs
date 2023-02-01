using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoad : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<SceneLoadTrigger>().LoadNewScene();
    }

}
