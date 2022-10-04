using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrandmaDialog : NPCBehavior
{ 
    private GrandmaDialog _grandmaWelcomeHome;
    private GrandmaDialogTea _grandmaTeaTime;
    [SerializeField] GameObject _teaTimeTriger;

    private void Start()
    {
        EventManager.OnEndDialog += NextDialogScript;

        _grandmaWelcomeHome = GetComponent<GrandmaDialog>();
        _grandmaTeaTime = GetComponent<GrandmaDialogTea>();
    }

    private void OnDisable()
    {
        EventManager.OnEndDialog -= NextDialogScript;
    }

    public override void SomeAction()
    {
        _teaTimeTriger.SetActive(true);
    }

    private void NextDialogScript()
    {
        _grandmaTeaTime.enabled = true;
        Destroy(_grandmaWelcomeHome);
    }
}
