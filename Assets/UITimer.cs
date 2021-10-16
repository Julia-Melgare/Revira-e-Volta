using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITimer : MonoBehaviour
{
    private float totalTime;
    private float timeRemaining;
    private TMPro.TextMeshProUGUI timeText;

    private void OnEnable()
    {
        LevelStateController.onSetTimer += GetLevelTime;
    }

    private void OnDisable()
    {
        LevelStateController.onSetTimer -= GetLevelTime;
    }

    void Start()
    {
        timeText = gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        timeRemaining = totalTime;
    }

    // Update is called once per frame
    void Update()
    {
        timeRemaining -= Time.deltaTime;
        if(timeRemaining > 0)
        {
            DisplayTime(timeRemaining);
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void GetLevelTime(float totalTime)
    {
        this.totalTime = totalTime;
    }
}
