using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehavior : MonoBehaviour, IInteracteable
{
    [SerializeField] private AudioSource _doorOpenedSound;
    private GameObject _mainCamera;
    // значение смещения камеры (половина размера комнаты, в данном случае 18f).
    [SerializeField] private float _camOffset;
    private Vector3 _direction;
    private float _canMove = 0;

    [SerializeField] private GameObject _enterPosition;
    private GameObject _character;

    [SerializeField] private string _textPrompt;

    public string PromptText => _textPrompt;

    private void Start()
    {
        _character = GameObject.FindGameObjectWithTag("Player");
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

        _direction = transform.localScale;
    }

    private void FixedUpdate()
    {
        CameraOffset();
    }

    // реализация интерфейса взаимодействия с объектами
    public bool GetInteracted(InteractionsBehaviour target)
    {
        DoorInteraction();
        EnterPosition();
        return true;
    }

    private void DoorInteraction()
    {
        // если персонаж взаимодействует с дверью, то
        // проиграть звук открытия двери
        _doorOpenedSound.Play();
        // дать возможность двигаться камере, увеличив значение _canMove
        _canMove = _camOffset;
    }

    // смещение камеры на центр комнаты, куда вошел персонаж
    private void CameraOffset()
    {
        if (_canMove > 0f)
        {
            _mainCamera.transform.position = new Vector3(_mainCamera.transform.position.x + (.5f * _direction.x),
                _mainCamera.transform.position.y, _mainCamera.transform.position.z);
            _canMove -= .5f;
        }
    }

    // перемещение персонажа в комнату, в которую он вошел
    private void EnterPosition()
    {
        _character.transform.position = new Vector3(_enterPosition.transform.position.x, _enterPosition.transform.position.y,
            _enterPosition.transform.position.z);
    }
}
