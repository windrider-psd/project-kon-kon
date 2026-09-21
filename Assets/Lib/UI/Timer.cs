using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


public class Timer
{
    private float duration;
    private float elapsed;

    public bool Finished => elapsed >= duration;

    public Timer(float seconds)
    {
        duration = seconds;
        elapsed = 0f;
    }

    public void Update()
    {
        elapsed += Time.deltaTime;
    }

    public void Reset()
    {
        elapsed = 0f;
    }
}

