using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractionsBehaviour : MonoBehaviour
{
    private IInteracteable _target;
    private Transform _targetPosition;
    [SerializeField] private Transform _interactCheck;
    [SerializeField] private LayerMask _interactLayer;
    [SerializeField] private float _radiusInteract;
    private Collider2D[] _colliders = new Collider2D[3];
    private int _interactFound;

    [SerializeField] private GameObject _popUpBubble;
    [SerializeField] private TextMeshProUGUI _textPrompt;

    private void Update()
    {
        // сколько коллизий с конкретным слоем произошло в данный момент
        _interactFound = Physics2D.OverlapCircleNonAlloc(_interactCheck.position, _radiusInteract, _colliders, _interactLayer);

        Interaction();
    }

    // само взаимодействие с объетктом коллизии
    private void Interaction()
    {
        // если объктов в зоне взаимодействия больше 0
        if (_interactFound > 0)
        {
            // то ссылаться на объект, у которого есть этот интерфейс
            _target = _colliders[0].GetComponent<IInteracteable>();
            _targetPosition = _colliders[0].GetComponent<Transform>();

            // проверяем наличие объектв в поле
            if (_target != null)
            { 
                // проверка тега объекта на взаимодействие по кнопке
                if (_colliders[0].CompareTag("Interact"))
                {
                    // вывести бабл с подсказкой над объектом
                    _popUpBubble.SetActive(true);
                    _textPrompt.text = _target.PromptText;
                    Transform child = _colliders[0].transform.GetChild(0);
                    float size = child.GetComponent<SpriteRenderer>().bounds.size.y;
                    _popUpBubble.transform.position = new Vector3(_targetPosition.transform.position.x, _targetPosition.transform.position.y + size, 
                        _targetPosition.transform.position.z);

                    if (Input.GetButtonDown("Interact"))
                    {
                        // запустить реализацию интерфейса взаимодействия
                        _target.GetInteracted(this);
                        // закрыть бабл
                        ClosePopUp();
                    }
                }
                // проверка тега объекта на то что его можно просто подобрать
                if (_colliders[0].CompareTag("CanCollect"))
                {
                    // запустить реализацию интерфейса
                    _target.GetInteracted(this);
                }
            }
        }
        else
        {
            // если объект все еще не нулл
            if (_target != null)
            {
                // закрыть бабл
                ClosePopUp();
                _target = null;
            }
        }
    }

    private void ClosePopUp()
    {
        //_target.PopUpPrompt(false);
        _popUpBubble.SetActive(false);
    }
}
