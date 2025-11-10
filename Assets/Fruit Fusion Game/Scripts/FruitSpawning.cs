using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class FruitSpawning : MonoBehaviour
{
    public GameObject DropParent;
    public GameObject DropingLine;
    public float maxLeftX = -3f;
    public float maxRightX = 3f;
    public float moveSpeed = 5f;
    public float fruitZ = -1f;
    public GameObject fruitsContainer;
    public GameObject DropFruit;
    public bool dropFruitCollided = true;
    private Vector2 isTouched;
    private Vector2 isTouchEnd;
    private bool isLeftRigth = false;
    private bool Moving = false;
    private float SwipeSensitivity = 0.005f;
    private float TapThreshold = 12f;
    private void Start()
    {
        Invoke(nameof(DropFirstFruit), 0.1f);
    }
    private void Update()
    {
        if (GameManager.instance.IsLevelFailed|| GameManager.instance.IsLevelPaused || GameManager.instance.IsLevelComplete) return;
        HandleMoveAndTouch();
        HideDropLine();
        if (CheckDroppedFruitCollision())
        {
            DropFruit = null;
            DropFirstFruit();
            GameManager.instance.UpdateNextFruitDisplay();
            GameManager.instance.CanDrop = true;
        }
    }
    void DropFirstFruit()
    {
        if (GameManager.instance.StartingFruits.Count == 0) return;

        GameObject first = GameManager.instance.StartingFruits[0];
        Vector3 pos = new Vector3(DropParent.transform.position.x, DropParent.transform.position.y, fruitZ);
        DropFruit = Instantiate(first, pos, Quaternion.identity);
        DropFruit.transform.SetParent(DropParent.transform);
        DropFruit.GetComponent<Rigidbody2D>().simulated = false;
    }
    // on click Fruit Fall
    /*    void HandleMoveAndTouch()
        {
            if (GameManager.instance.IsLevelFailed) return;
            Vector3 pos = transform.position;
            // ---------------- MOBILE TOUCH ----------------
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        isTouched = touch.position;
                        isLeftRigth = true;
                        Moving = false;
                        break;

                    case TouchPhase.Moved:
                        if (isLeftRigth)
                        {
                            float deltaX = touch.deltaPosition.x * SwipeSensitivity;
                            pos.x += deltaX * moveSpeed;
                            Moving = true; // we moved, so it's not a tap
                        }
                        break;

                    case TouchPhase.Ended:
                        isTouchEnd = touch.position;
                        float distance = Vector2.Distance(isTouched, isTouchEnd);

                        // Only drop if it's a TAP (no move)
                        if (!Moving && distance < TapThreshold)
                        {
                            if (GameManager.instance.CanDrop)
                                FruitFallSystem();
                        }

                        isLeftRigth = false;
                        Moving = false;
                        break;
                }
            }

            pos.x = Mathf.Clamp(pos.x, maxLeftX, maxRightX);
            transform.position = pos;
        }*/

    // auto fall when leave touch
    void HandleMoveAndTouch()
    {
        if (GameManager.instance.IsLevelFailed) return;

        Vector3 pos = transform.position;

        // ---------------- MOBILE TOUCH ----------------
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    isTouched = touch.position;
                    isLeftRigth = true;
                    Moving = false;
                    break;

                case TouchPhase.Moved:
                    if (isLeftRigth)
                    {
                        float deltaX = touch.deltaPosition.x * SwipeSensitivity;
                        pos.x += deltaX * moveSpeed;
                        Moving = true;
                    }
                    break;

                case TouchPhase.Ended:
                    // When user lifts finger — drop the fruit automatically
                    if (GameManager.instance.CanDrop)
                    {
                        FruitFallSystem();
                    }

                    isLeftRigth = false;
                    Moving = false;
                    break;
            }
        }

        pos.x = Mathf.Clamp(pos.x, maxLeftX, maxRightX);
        transform.position = pos;
    }
    void FruitFallSystem()
    {
        if (DropFruit == null) return;

        dropFruitCollided = false;
        DropFruit.transform.SetParent(fruitsContainer.transform);
        DropFruit.GetComponent<Rigidbody2D>().simulated = true;
        GameManager.instance.CanDrop = false;

        GameManager.instance.StartingFruits.RemoveAt(0);
        GameManager.instance.AddRandomFruitToQueue();
    }
    bool CheckDroppedFruitCollision()
    {
        if (DropFruit == null || dropFruitCollided) return false;
        Collider2D col = DropFruit.GetComponent<Collider2D>();
        if (col == null) return false;

        Collider2D[] cols = Physics2D.OverlapBoxAll(col.bounds.center, col.bounds.size, 0f);
        foreach (Collider2D c in cols)
        {
            if (c.gameObject == DropFruit) continue;
            if (c.CompareTag("Ground") || c.CompareTag("Fruit"))
            {
                dropFruitCollided = true;
                return true;
            }
        }
        return false;
    }
    void HideDropLine()
    {
        if (DropingLine != null)
            DropingLine.GetComponent<SpriteRenderer>().enabled = GameManager.instance.CanDrop;
    }
}
