using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportBehaviourScript : MonoBehaviour, IInteracteable
{
    [SerializeField] private string _textPrompt;
    private GameObject _character;
    [SerializeField] private GameObject _newPoint;
    [SerializeField] private AudioSource _SFX;
    [SerializeField] private GameObject _fade;
    [SerializeField] private GameObject _bright;
    public string PromptText => _textPrompt;

    private void Start()
    {
        _character = GameObject.FindGameObjectWithTag("Player");
    }

    public bool GetInteracted(InteractionsBehaviour target)
    {
        Teleportation();
        return true;
    }

    private void Teleportation()
    {
        Fading();
        Invoke("Brighting", 1.5f);
    }

    private void Fading()
    {
        _fade.SetActive(true);
        _bright.SetActive(false);
        _SFX.Play();
    }

    private void Brighting()
    {
        _character.transform.position = new Vector3(_newPoint.transform.position.x, _newPoint.transform.position.y,
    _newPoint.transform.position.z);
        _fade.SetActive(false);
        _bright.SetActive(true);
    }
}
