using System.Collections.Generic;
using UnityEngine;

public class GameLose : MonoBehaviour
{
    public float TimeTakenToFail = 3f;
    public float BlinkingTime = 0.8f;
    [Header("Blinking settings")]
    public Color FailBlinkStartColor = Color.white; // Full alpha color
    public Color FailBlinkEndColor = new Color(1f, 1f, 1f, 0f); // 0 alpha color
    public float OnFailedBlinkSpeed = 5f;
    private Dictionary<GameObject, float> OnLevelFailedFruits;

    void Start()
    {
        OnLevelFailedFruits = new Dictionary<GameObject, float>();
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Fruit")) return;

        float currentTime = OnLevelFailedFruits.GetValueOrDefault(collision.gameObject, 0f);
        OnLevelFailedFruits[collision.gameObject] = currentTime + Time.deltaTime;
        if (currentTime + Time.deltaTime >= BlinkingTime)
        {
            MakeFruitBlink(collision.gameObject);
        }
        if (currentTime + Time.deltaTime >= TimeTakenToFail)
        {
            if (GameManager.instance.IsLevelFailed != true)
            {
                GameManager.instance.IsLevelFailed = true;
                GameManager.instance.FailedPanel.SetActive(true);

            }
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        OnLevelFailedFruits.Remove(collision.gameObject); // Remove object from losingFruits dic
        StopFruitBlink(collision.gameObject); // Revert fruit to normal color if was blinking
    }
    void MakeFruitBlink(GameObject gameObject)
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.Lerp(FailBlinkStartColor, FailBlinkEndColor, Mathf.PingPong(Time.time * BlinkingTime, 1));
    }
    void StopFruitBlink(GameObject gameObject)
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.Lerp(gameObject.GetComponent<SpriteRenderer>().color, FailBlinkStartColor, 1f);
    }

}
