using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
public class CameraControl : MonoBehaviour
{
    public Transform Knife;
    // public GameObject[] Fruits;
    public Text Count;
    [SerializeField]private bool stop;
    public static int scoreadd;
    public float time;
    //  public AudioSource As;
   // public AudioClip AC;
    public float wait;
    public GameObject Tuturial;
    public Vector3 spawnPosition;
    void Start()
    {
        Count.text = scoreadd.ToString();
        Input.simulateMouseWithTouches = true;
        QualitySettings.shadowDistance = 10;
        Input.multiTouchEnabled = false;
        QualitySettings.shadowDistance = 10;
        ResetTime();
        StartCoroutine(IncreaseInTime());
    }
    void Update()
    {
        if (Input.GetMouseButton(0) && stop == false && Input.mousePosition.y < Screen.height / 2)
        {
            StartCoroutine(StopTheAnimation());
        }
    }
    private IEnumerator IncreaseInTime()
    {
        if (Time.timeScale < 5)
            Time.timeScale += time;
        yield return new WaitForSeconds(5f);
        StartCoroutine(IncreaseInTime());
    }
    private IEnumerator StopTheAnimation()
    {
        Tuturial.SetActive(false);
        stop = true;
        Knife.GetComponent<Animator>().Play("Fill");
       // As.PlayOneShot(AC);
        yield return new WaitForSeconds(wait);
        stop = false;
    }
    public void ResetTime()
    {
        Time.timeScale = 1;
    }
}
