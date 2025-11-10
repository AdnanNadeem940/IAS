using Pixelplacement;
using UnityEngine;
public class GameController : Singleton<GameController>
{
    public static GameController instance;
    public bool IsSpaceAvialable;
    public UnScrewNuts  PreviousNut, NextNut;
    public ScrewController SelectedScrew;
    private void Awake()
    {
        instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
