using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BankSystem : MonoBehaviour
{
    private int peachCounter = 0;
    [SerializeField] private AudioSource _peachUpSound;

    private void Start()
    {
        EventManager.OnPeachUp += PeachUp;
    }

    private void PeachUp()
    {
        peachCounter++;
        _peachUpSound.Play();
        GetComponent<Text>().text = peachCounter.ToString();
    }
}
