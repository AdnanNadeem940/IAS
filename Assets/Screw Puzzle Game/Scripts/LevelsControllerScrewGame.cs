using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelsControlllerScrewGame : MonoBehaviour
{
    public static LevelsControlllerScrewGame Instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject[] LevelsHolder;
    [Header("Panels")]
    public GameObject LevelCompletePanel, LevelFailedPanel, PausePanel;
    int CurrentLevel;
    float TimeRemain=180;
    public bool TimeIsRunning;
    public Text timeText, WinScore, LoseScore;
    public Button NextBtn, ResterBtn, RestartBtnPauseScreen, PauseBtn, ResumeBtn;
    void Start()
    {
        Instance = this;
        CurrentLevel = PlayerPrefs.GetInt("CurrentLevelScrew");
        LevelsHolder[CurrentLevel].SetActive(true);
        TimeIsRunning = true;
        StartCoroutine(TimerCalculate());
        AddButtonEvents();
    }
    void AddButtonEvents()
    {
        NextBtn.onClick.AddListener(NextLevel);
        ResterBtn.onClick.AddListener(LevelRestart);
        RestartBtnPauseScreen.onClick.AddListener(LevelRestart);
        PauseBtn.onClick.AddListener(LevelPause);
        ResumeBtn.onClick.AddListener(LevelResume);
    }
    IEnumerator TimerCalculate()
    {
        while (TimeIsRunning)
        {
            Debug.Log("Time Coroutine Enter");
            if (TimeIsRunning)
            {
                Debug.Log("Time Coroutine is Working");
                if (TimeRemain >= 0)
                {
                    TimeRemain -= 1;
                    DisplayTime(TimeRemain);
                    yield return new WaitForSeconds(1);
                }
                else
                {
                    TimeIsRunning = false;
                    TimeRemain = 0;
                    DisplayTime(TimeRemain);
                    LevelFailed();
                }
            }
        }
    }
    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = "Time Remain =" + string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    public void LevelComplete()
    {
        LevelsHolder[CurrentLevel].SetActive(false);
        PlayerPrefs.SetInt("CurrentLevelScrew", PlayerPrefs.GetInt("CurrentLevelScrew") + 1);
        PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + 500);
        WinScore.text= PlayerPrefs.GetInt("Coins").ToString();
        LevelCompletePanel.SetActive(true);
    }
    void NextLevel()
    {
        CurrentLevel = PlayerPrefs.GetInt("CurrentLevelScrew");
        if (CurrentLevel < 5)
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }
    public void LevelFailed()
    {
        LevelsHolder[CurrentLevel].SetActive(false);
        LoseScore.text= PlayerPrefs.GetInt("Coins").ToString();
        LevelFailedPanel.SetActive(true);
    }
    void LevelRestart()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
    void LevelPause()
    {
        TimeIsRunning = false;
        PausePanel.SetActive(true);
    }    
    void LevelResume()
    {
        TimeIsRunning= true;
        PausePanel.SetActive(false);
    }
}
