using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Timer : MonoBehaviour
{
    bool timerActive = false;
    public float currentTime;
    [SerializeField] public int startMin;
    [SerializeField] public Text CurrentTimeText;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = startMin * 60;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerActive)
        {
            currentTime = currentTime + Time.deltaTime;
        }
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        CurrentTimeText.text = time.Minutes.ToString() + ":" + time.Seconds.ToString();
    }

    public void StartTimer()
    {
        timerActive = true;
    }

    public void EndTimer()
    {
        timerActive = false;
    }
}
