using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDisable : MonoBehaviour
{
    private GameObject _character;
    private CharacterControl _controller;
    [SerializeField] private bool _isCanMove;

    private void Start()
    {
        _character = GameObject.Find("Character");
        _controller = _character.GetComponent<CharacterControl>();
    }

    private void Update()
    {
        _controller.CanMove = _isCanMove;
    }
}
