using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPCBehavior : MonoBehaviour, IInteracteable
{
    [SerializeField] 
    private TextAsset _textDialog;
    [SerializeField] 
    private GameObject _canvas;
    [SerializeField] 
    private InkManager _ink;
    [SerializeField]
    private string _nameNPC;
    [SerializeField]
    private string _textPrompt;

    public string PromptText => _textPrompt;

    //[SerializeField] private GameObject _popUpText;

    public bool GetInteracted(InteractionsBehaviour target)
    {
        if (!_ink._blockInteractions)
        {
            ActivateDialog();
            SomeAction();
        }
        return true;
    }

    public abstract void SomeAction();

    private void ActivateDialog()
    {
        _ink.NewStory(_textDialog);
        _ink._nameField.text = _nameNPC;
        _canvas.SetActive(true);
        _ink._blockInteractions = true;
    }
}
