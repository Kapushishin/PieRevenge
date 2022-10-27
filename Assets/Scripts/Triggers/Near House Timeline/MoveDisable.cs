using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDisable : MonoBehaviour
{
    private GameObject _character;
    private CharacterControl _controller;
    private InkManager _ink;
    [SerializeField] private bool _isCanMove;

    private void Start()
    {
        _character = GameObject.Find("Character");
        _controller = _character.GetComponent<CharacterControl>();
        var canvas = GameObject.Find("Canvas Dialogs");
        _ink = canvas.GetComponent<InkManager>();
    }

    private void Update()
    {
        _controller.CanMove = _isCanMove;
        _ink.BlockInteractions = !_isCanMove;
    }
}
