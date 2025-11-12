using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
public class GameControllerNutSort : MonoBehaviour
{
    public static GameControllerNutSort Instance;
    private Bolt CurrentScrew;
    private Nut CurrentNut;
    public bool CanMove = true;
    public int moveTimes;
    public int MoveTimes
    {
        get { return moveTimes; }
        set { moveTimes = value; }
    }
    public UnityEvent<int> OnMoveTimesChanged;
    public UnityEvent OnLevelComplete;
    public UnityEvent OnLevelFailed;
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && CanMove)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f))
            {
                if (hit.collider.gameObject.TryGetComponent<Bolt>(out Bolt screw))
                {
                    if (CurrentScrew == null)
                    {
                        FirstSelect(screw);
                    }
                    else
                    {
                        SecondSelect(screw);
                    }
                }
            }
        }
    }
    private IEnumerator MoveAllNutToNewNut(Color moveColor, Bolt bolt)
    {
        MoveController.Instance.UpdatReverseButtonStatus(false);
        CanMove = false;
        int timesmove = 0;
        while (!bolt.IsFull())
        {
            Nut topNut = CurrentScrew.GetTopNut();
            if (topNut == null || topNut.color != moveColor)
                break;

            CurrentScrew.DetachNut(topNut);

            yield return new WaitForSeconds(0.2f);
            topNut.transform.SetParent(bolt.transform);
            CurrentScrew.RemoveTopNut();
            StartCoroutine(bolt.MoveNutToThisScrew(topNut));

            yield return new WaitForSeconds(0.01f); // 
            timesmove++;
        }

        yield return new WaitForSeconds(0.5f);

        MoveController.Instance.AddMove(new Move(CurrentScrew, bolt, timesmove));
        CurrentNut = null;
        CurrentScrew = null;
        CanMove = true;
        MakeMove();
    }
    public void FirstSelect(Bolt bolt)
    {
        Nut top = bolt.GetTopNut();
        if (top != null)
        {
            bolt.DetachNut(top);
            CurrentNut = top;
            CurrentScrew = bolt;
            //SoundManager.Instance.PopSFX();
        }
    }
    

    public void SecondSelect(Bolt bolt)
    {
        if (bolt == CurrentScrew)
        {
            CurrentScrew.InsertNut(CurrentNut);
            CurrentNut = null;
            CurrentScrew = null;
            return;
        }
        Nut top = bolt.GetTopNut();
        if (top == null)
        {
            StartCoroutine(MoveAllNutToNewNut(CurrentNut.color, bolt));
        }
        else
        {
            if (top.color != CurrentNut.color || bolt.IsFull())
            {
                CurrentScrew.InsertNut(CurrentNut);
                CurrentNut = null;
                CurrentScrew = null;
            }
            else
            {
                StartCoroutine(MoveAllNutToNewNut(CurrentNut.color, bolt));
            }
        }
    }
    public void MakeMove()
    {
        MoveTimes--;
        LevelManager.Instance.CheckGameState();
    }
    public void AddMove(int times)
    {
        MoveTimes += times;
    }
    public void NewGame()
    {
        MoveController.Instance.ResetMoves();
        CanMove = true;
        MoveTimes = LevelManager.Instance.currLevelObject.MoveTime;
    }

    public void Victory()
    {
        Debug.LogError("Level Completed");
    }

    public void WinGame()
    {
        LevelManager.Instance.ChangeLevelPassed();;
        OnLevelComplete?.Invoke();
        CanMove = false;
    }

    public void LoseGame()
    {
        OnLevelFailed?.Invoke();
        CanMove = false;
    }
}
