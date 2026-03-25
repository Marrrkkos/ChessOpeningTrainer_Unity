using UnityEngine;
using UnityEngine.UI;
public class Timer : MonoBehaviour
{
    [Header("Timer Einstellungen")]
    public float timeRemaining = 60f;
    public bool timerIsRunning = false;

    [Header("UI Referenz")]
    public Text timeText;

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                Debug.Log("Die Zeit ist abgelaufen!");
                timeRemaining = 0;
                timerIsRunning = false;
                timeText.text = "00:00";
            }
        }
    }
    void DisplayTime(float timeToDisplay)
    {
        if(timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }

        float minutes = Mathf.FloorToInt(timeToDisplay / 60); 
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
