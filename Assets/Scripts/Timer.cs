using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TMP_Text timerObject;

    public void SetTimer(int time)
    {
        int minutes = time / 60;
        int seconds = time % 60;
        string timer = minutes + " : " + seconds;
        timerObject.text = timer;
    }
}
