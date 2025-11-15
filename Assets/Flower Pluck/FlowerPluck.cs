using System;
using UnityEngine;
using UnityEngine.UI;

public class FlowerPluck : MonoBehaviour
{
    public Camera CameraMain;
    public LayerMask SelectedLayer;
    public int PluckedFlower, TargetFlower;
    public GameObject YellowFlower, RedFlower;
    public Text ScoreText;
    public GameObject[] Array;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int Score = PlayerPrefs.GetInt("Score");
        ScoreText.text= Score.ToString();
        if (PlayerPrefs.GetInt("FlowerUnlocked")==1)
        {
            YellowFlower.SetActive(false);
            RedFlower.SetActive(true);
            TargetFlower = 44;
        }
        else
        {
            TargetFlower = 24;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Only once per click
        {
            Ray ray = CameraMain.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000, SelectedLayer))
            {
                Rigidbody rb = hit.transform.GetComponent<Rigidbody>();
                MeshCollider mc = hit.transform.GetComponent<MeshCollider>();

                if (rb != null)
                {
                    rb.isKinematic = false;

                    // Random force to make leaf fall in a natural direction
                    Vector3 randomForce = new Vector3(
                        UnityEngine.Random.Range(-1f, 1f),
                        UnityEngine.Random.Range(2f, 4f),
                        UnityEngine.Random.Range(-1f, 1f)
                    );

                    // Random spin torque for tumbling effect
                    Vector3 randomTorque = new Vector3(
                        UnityEngine.Random.Range(-10f, 10f),
                        UnityEngine.Random.Range(-10f, 10f),
                        UnityEngine.Random.Range(-10f, 10f)
                    );

                    // Apply them
                    rb.AddForce(randomForce, ForceMode.Impulse);
                    rb.AddTorque(randomTorque, ForceMode.Impulse);
                }

                if (mc != null)
                    mc.enabled = false;

                // Optional slight backward offset (visual detachment)
                hit.transform.position -= new Vector3(0, 0, 0.1f);
                PluckedFlower++;
                UpdateScore();
                if (PluckedFlower >= TargetFlower)
                {
                    Complete();
                }
            }
        }
    }
    void UpdateScore()
    {
        int Score = PlayerPrefs.GetInt("Score");
        Score = Score + 50;
        PlayerPrefs.SetInt("Score", Score);
        ScoreText.text= Score.ToString();
    }
    public void Complete()
    {
        Debug.Log("Level Complete You Win");
        if(PlayerPrefs.GetInt("FlowerUnlocked")==0)
        {
            YellowFlower.SetActive(false);
            RedFlower .SetActive(true);
            PlayerPrefs.SetInt("FlowerUnlocked", 1);
            TargetFlower = 44;
            PluckedFlower = 0;
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(0);
        }
    }
    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
