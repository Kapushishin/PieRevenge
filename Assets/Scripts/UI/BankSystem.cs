using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BankSystem : MonoBehaviour
{
    public static int _peachCounter = 0;
    [SerializeField] private AudioSource _peachUpSound;

    private void Start()
    {
        EventManager.OnPeachUp += PeachUp;
    }

    private void PeachUp()
    {
        _peachCounter++;
        _peachUpSound.Play();
        GetComponent<Text>().text = _peachCounter.ToString();
    }
}
