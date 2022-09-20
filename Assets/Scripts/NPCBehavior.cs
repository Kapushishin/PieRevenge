using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPCBehavior : MonoBehaviour, IInteracteable
{
    [SerializeField] 
    private TextAsset _textDialog;
    protected GameObject _canvas;
    protected InkManager _ink;
    [SerializeField]
    private string _nameNPC;
    [SerializeField]
    private string _textPrompt;
    [SerializeField]
    public bool _isDisableable;
    private bool _canTalk = true;
    public string PromptText => _textPrompt;

    private void Awake()
    {
        _canvas = GameObject.Find("Canvas Dialogs");
        _ink = _canvas.gameObject.GetComponent<InkManager>();
    }

    public bool GetInteracted(InteractionsBehaviour target)
    {
        if (!_ink.BlockInteractions && _canTalk)
        {
            ActivateDialog();
            SomeAction();
            if (_isDisableable)
            {
            _canTalk = false;
            }
        }
        return true;
    }

    public abstract void SomeAction();

    private void ActivateDialog()
    {
        _ink.NewStory(_textDialog);
        _ink.NameField.text = _nameNPC;
        _canvas.SetActive(true);
        _ink.BlockInteractions = true;
    }
}
