using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneController : MonoBehaviour
{
    public static MainSceneController Instance;
    public GameObject SettingPanel;
    private void Awake()
    {
        Instance = this;
    }
    public void SelectGameID(int ScendID)// start from 1 to ownwards
    {
        SceneManager.LoadScene(ScendID);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnSettingButtonClicked()
    {

    }
}
