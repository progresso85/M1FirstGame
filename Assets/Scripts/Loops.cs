using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System;

public class Loops : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI loopObject;

    public void Update()
    {
        SetLoops(GameManager.Instance.loops);
    }

    public void SetLoops(int loops)
    {
        
        string loopText = loops + "";
        loopObject.text = loopText;

    }
}
