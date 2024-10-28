using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerObject;

    public void Update()
    {
        SetTimer(GameManager.Instance.timer);
    }

    public void SetTimer(float time)
    {
        double doubleTime = Math.Round(time, 0);
        int minutes = Convert.ToInt32(doubleTime) / 60;
        int seconds = Convert.ToInt32(doubleTime) % 60;
        string timer;
        if (seconds < 10)
        {
            timer = minutes + " : 0" + seconds;
        }
        else
        {
            timer = minutes + " : " + seconds;
        }

  
        timerObject.text = timer;

    }
}
