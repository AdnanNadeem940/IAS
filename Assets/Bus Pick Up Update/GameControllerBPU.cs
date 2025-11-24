using DG.Tweening;
using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class GameControllerBPU : MonoBehaviour
{
    public static GameControllerBPU Instance;
    public GameObject CompleteScreen, FailedScreen;
    public TextMeshProUGUI LevelNumberText;
    #region Timer 
    public GameObject[] LevelsBusPickUp;
    public float timeRemaining = 100;
    public TMP_Text timerText;
    public GameObject TimeOutPanel;
    bool isTimerBlinking;
    [SerializeField]
    RectTransform timerIntroducePanel;
    bool isTimerShown, CanCalculateTime;
    int ActiveLevel;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ActiveLevel = PlayerPrefs.GetInt("PickUpLevel");
        timeRemaining = timeRemaining + (ActiveLevel * 10);
        LevelNumberText.text="Level"+"\n"+(ActiveLevel+1).ToString();
        Instantiate(LevelsBusPickUp[ActiveLevel], LevelsBusPickUp[ActiveLevel].transform.position, Quaternion.identity);
  
        isTimerShown = (PlayerPrefs.GetInt("TimerShown") != 0);
        if (!isTimerShown)
        {
            timerIntroducePanel.anchoredPosition = new Vector2(0, -timerIntroducePanel.rect.height);
            timerIntroducePanel.DOAnchorPosY(250, 0.5f).SetEase(Ease.OutQuad);
            isTimerShown = true;
            PlayerPrefs.SetInt("TimerShown", (isTimerShown ? 1 : 0));
            UpdateTimerDisplay();
        }
        else
        {
            UpdateTimerDisplay();
            AllowTimeToCalCulate();
        }
    }

    public void AllowTimeToCalCulate()
    {
        CanCalculateTime = true;
        timerIntroducePanel.gameObject.SetActive(false);
    }
    void Update()
    {
        if (CanCalculateTime)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerDisplay();

                if (timeRemaining <= 10.0f && !isTimerBlinking)
                {

                    StartCoroutine(BlinkText());
                    isTimerBlinking = true;
                }
            }
            else
            {
                EndTime();
                StopCoroutine(BlinkText());
                timerText.color = Color.red;
            }
        }
        }

        void UpdateTimerDisplay()
    {

        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void EndTime()
    {
        CanCalculateTime = false;
        timerText.text = "00:00";
        timeRemaining = 0;
        TimeOutPanel.SetActive(true);
       /* TimeOutPanel.transform.localScale = Vector3.zero;
        TimeOutPanel.transform.DOScale(1, 1f).SetEase(Ease.OutBack);*/

    }

    public void TurnOffTimerIntroduceManager()
    {
        timerIntroducePanel.gameObject.SetActive(false);
    }


    IEnumerator BlinkText()
    {
        while (true)
        {
            timerText.color = Color.red;
            yield return new WaitForSeconds(0.5f);
            timerText.color = Color.white;
            yield return new WaitForSeconds(0.5f);
        }
    }

    #endregion
    public void NextLevelLoad()
    {
        if (ActiveLevel < LevelsBusPickUp.Length)
        {
            ActiveLevel = PlayerPrefs.GetInt("PickUpLevel");
            ActiveLevel += 1;
            timeRemaining = timeRemaining + 10;
            PlayerPrefs.SetInt("PickUpLevel", ActiveLevel);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    public void LevelComplete()
    {
        CanCalculateTime = false;
        if (CompleteScreen != null)
            CompleteScreen.SetActive(true);
        CompleteScreen.transform.localScale = Vector3.zero;
        CompleteScreen.transform.DOScale(1, 1f).SetEase(Ease.OutBack);
        CompleteScreen.SetActive(true);
    }
    public void LevelFailed()
    {
        CanCalculateTime = false;
        FailedScreen.SetActive(true);
    }
    public void RestartCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void BackHome()
    {
        SceneManager.LoadScene(0);
    }
}
