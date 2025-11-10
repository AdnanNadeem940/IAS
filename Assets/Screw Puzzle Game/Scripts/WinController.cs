using System.Collections.Generic;
using UnityEngine;

public class WinController : MonoBehaviour
{
    public int WinCount;
    int collisionCount;
    private HashSet<GameObject> triggeredObjects = new HashSet<GameObject>();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Win"))
        {
            GameObject parentObject = collision.gameObject;
            if (triggeredObjects.Contains(parentObject))
                return;
            triggeredObjects.Add(parentObject);
            parentObject.SetActive(false);
            collisionCount++;
            if (collisionCount == WinCount)
            {
                Debug.Log("User Win Congrats");
                LevelsControlllerScrewGame.Instance.LevelComplete();
            }
        }
    }
}
   
