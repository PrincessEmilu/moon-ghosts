using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The Timer class is responsibly solely for keeping track of time passing.
// Each time Tick() is called, the seconds since the last frame are recorded.
// CheckTime() will return true ifthe specified amount of time has passed since initilaized or reset.
// Reset() resets the timer count to 0
public class Timer
{
    // The current count of the timer, in seconds
    private float m_timerCurrent;
    // The maximum count of the timer
    private float m_timerMax;

    /// <summary>
    /// Creates a new timer that will count up to the specified value in seconds.
    /// </summary>
    /// <param name="secondsToCount">The timer value to count up to, in seconds.</param>
    public Timer(float secondsToCount)
    {
        m_timerCurrent = 0;
        m_timerMax = secondsToCount;
    }

    /// <summary>
    /// Resets the timer to 0 so that it can count up again
    /// </summary>
    public void Reset()
    {
        m_timerCurrent = 0.0f;
    }

    /// <summary>
    /// Adds the time passed since the last Tick() call to the timer
    /// </summary>
    public void Tick()
    {
        m_timerCurrent += Time.deltaTime;
    }

    /// <summary>
    /// Checks to see if the timer has counted up or past the timer max.
    /// </summary>
    /// <returns>Returns true if the timer has ticked past the maximum time.</returns>
    public bool CheckTime()
    {
        return m_timerCurrent >= m_timerMax;
    }
}
