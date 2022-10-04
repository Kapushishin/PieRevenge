using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BankSystem : MonoBehaviour
{
    [SerializeField] private AudioSource _peachUpSound;

    private void Start()
    {
        EventManager.OnPeachUp += PeachUp;
        GetComponent<Text>().text = SwitchParametres.PeachCounter.ToString();
    }
    private void OnDisable()
    {
        EventManager.OnPeachUp -= PeachUp;
    }

    private void PeachUp()
    {
        SwitchParametres.PeachCounter++;
        _peachUpSound.Play();
        GetComponent<Text>().text = SwitchParametres.PeachCounter.ToString();
    }
}
