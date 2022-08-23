using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBehavior : MonoBehaviour, IInteracteable
{
    [SerializeField] 
    private TextAsset _textDialog;
    [SerializeField] 
    private GameObject _canvas;
    [SerializeField] 
    private InkManager _ink;
    [SerializeField] 
    private bool _inRange;
    [SerializeField]
    private string _nameNPC;

    private void Update()
    {
        if (!_ink._blockInteractions)
        {
            ActivateDialog();
        }
    }

    public bool GetInteracted(InteractionsBehaviour target)
    {
        _inRange = true;
        return true;
    }

    private void ActivateDialog()
    {
        if (_inRange && Input.GetButtonDown("Interact"))
        {
            _ink.NewStory(_textDialog);
            _ink._nameField.text = _nameNPC;
            _canvas.SetActive(true);
            _ink._blockInteractions = true;
        }
    }
}
