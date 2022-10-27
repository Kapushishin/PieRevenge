using UnityEngine;

public class ObjectsEnable : MonoBehaviour, IInteracteable
{
    [SerializeField] private GameObject[] _objectsEnable;
    [SerializeField] private GameObject[] _objectsDisable;

    [SerializeField] private bool _isTrigger;

    public string PromptText => "";

    private void Start()
    {
        if (!_isTrigger)
        {
            ObjectsBehavior();
        }
    }

    public bool GetInteracted(InteractionsBehaviour target)
    {
        ObjectsBehavior();
        return true;
    }

    private void EnableObjects()
    {
        for (int i = 0; i < _objectsEnable.Length; i++)
        {
            _objectsEnable[i].SetActive(true);
        }
    }

    private void DisableObjects()
    {
        for (int i = 0; i < _objectsDisable.Length; i++)
        {
            _objectsDisable[i].SetActive(false);
        }
    }

    private void ObjectsBehavior()
    {
        EnableObjects();
        DisableObjects();
        Destroy(gameObject);
    }
}
