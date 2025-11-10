using System.Collections.Generic;
using UnityEngine;

public class GameWin : MonoBehaviour
{
    public GameObject GameObjectToFailed;
    public float warningTime = 1f;

    private Dictionary<GameObject, float> WarningFruitsTimes;
    private List<GameObject> WarningFruits;
    private void Start()
    {
        WarningFruitsTimes = new Dictionary<GameObject, float>();
        WarningFruits = new List<GameObject>();
        GameObjectToFailed.GetComponent<SpriteRenderer>().enabled = false;
    }
    void Update()
    {
        if (WarningFruits.Count > 0) GameObjectToFailed.GetComponent<SpriteRenderer>().enabled = true;
        else GameObjectToFailed.GetComponent<SpriteRenderer>().enabled = false;
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Fruit")) return;

        float currentTime = WarningFruitsTimes.GetValueOrDefault(collision.gameObject, 0f);
        WarningFruitsTimes[collision.gameObject] = currentTime + Time.deltaTime;
        if (currentTime + Time.deltaTime >= warningTime)
        {
            if (!WarningFruits.Contains(collision.gameObject))
            {
                WarningFruits.Add(collision.gameObject);
            }
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        WarningFruitsTimes.Remove(collision.gameObject);
        WarningFruits.Remove(collision.gameObject);
    }
}
