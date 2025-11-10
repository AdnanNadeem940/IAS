using UnityEngine;
using UnityEngine.Playables;

public class PlayerHandler : MonoBehaviour
{
    public enum PlayerCondition
    {
        Normal,
        Happy,
        Sad
    }
    public PlayerCondition PlayerState = PlayerCondition.Normal;

    [Header("Player State Images")]
    public Sprite Normal;
    public Sprite Happy;
    public Sprite Sad;
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        UpdatePlayerStatus();
        UpdatePlayerState();
    }
    void UpdatePlayerStatus()
    {
        if (PlayerState == PlayerCondition.Normal) this.GetComponent<SpriteRenderer>().sprite = Normal;
        else if (PlayerState == PlayerCondition.Happy) this.GetComponent<SpriteRenderer>().sprite = Happy;
        else if (PlayerState == PlayerCondition.Sad) this.GetComponent<SpriteRenderer>().sprite = Sad;
    }
    void UpdatePlayerState()
    {
        // Happy
        if (!GameManager.instance.CanDrop) PlayerState = PlayerCondition.Happy;
        else PlayerState = PlayerCondition.Normal;

        // Sad
        if (GameManager.instance.IsLevelFailed) PlayerState = PlayerCondition.Sad;
    }
}
