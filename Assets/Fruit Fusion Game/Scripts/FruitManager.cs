using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class FruitManager : MonoBehaviour
{
    public int FruitIndex;
    public bool CanCollideWithSameType;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    void OnCollisionEnter2D(Collision2D collision) 
    {
        OnFruitsCollide(collision);
    }
    void OnCollisionStay2D(Collision2D collision) 
    { 
        OnFruitsCollide(collision);
    }
    void OnFruitsCollide(Collision2D collision)
    {
        FruitManager NewFruit = collision.gameObject.GetComponent<FruitManager>();
        if (CanCollideWithSameType) return;
        if (!collision.gameObject.CompareTag("Fruit")) return;
        if (NewFruit == null) return;
        if(NewFruit.CanCollideWithSameType) return;
        if (NewFruit.FruitIndex != FruitIndex) return;
        CanCollideWithSameType = true;
        NewFruit.CanCollideWithSameType = true;
        GameManager.instance.SameFruitCollided(gameObject, collision.gameObject, FruitIndex);

    }
}
