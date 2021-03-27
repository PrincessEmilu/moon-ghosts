using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    private float m_timerCurrent;
    private float m_timerMax;

    public Timer(float secondsToCount)
    {
        m_timerCurrent = 0;
        m_timerMax = secondsToCount;
    }

    public void Reset()
    {
        m_timerCurrent = 0.0f;
    }

    public void Tick()
    {
        m_timerCurrent += Time.deltaTime;
    }

    public bool CheckTime()
    {
        return m_timerCurrent >= m_timerMax;
    }
}
