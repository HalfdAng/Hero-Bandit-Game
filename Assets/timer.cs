using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;

public class timer : MonoBehaviour
{
    public bool timerActice;
    private float currentTime;
    [SerializeField] private TMP_Text text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentTime = 0;

        timerActice = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerActice == true)
        {
            currentTime = currentTime + Time.deltaTime;
        }

        TimeSpan time = TimeSpan.FromSeconds(currentTime);

        text.text = time.Minutes.ToString() + ":" + time.Seconds.ToString() + "." + time.Milliseconds.ToString();
    }
}
