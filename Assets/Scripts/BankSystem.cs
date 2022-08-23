using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BankSystem : MonoBehaviour
{
    private int peachCounter = 0;

    private void Start()
    {
        EventManager.OnPeachUp += PeachUp;
    }

    private void PeachUp()
    {
        peachCounter++;
        GetComponent<Text>().text = peachCounter.ToString();
    }
}
