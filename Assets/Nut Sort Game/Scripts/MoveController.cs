using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MoveController : MonoBehaviour
{
    public static MoveController Instance;
    private Stack<Move> moves;
    [SerializeField] private Button ReverseButton;
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        moves = new Stack<Move>();
        ReverseButton.onClick.AddListener(UndoMove);
        UpdatReverseButtonStatus(false);
    }
 
    public void UndoMove()
    {
        int Coins = PlayerPrefs.GetInt("Score");
        if (Coins<= 100)
        {
            Debug.Log("You Dont Have Enough Coins");
            return;
        }
        if (moves.TryPop(out Move move))
        {
            PlayerPrefs.SetInt("Score", Coins - 100);

            for (int i = 0; i < move.number; i++)
            {
                Nut topNut = move.toScrew.GetTopNut();
                if (topNut == null) break;
                topNut.transform.SetParent(move.fromScrew.transform);
                move.toScrew.RemoveTopNut();
                move.fromScrew.AddNutNoAnimation(topNut);
                GameControllerNutSort.Instance.AddMove(1);
            }
        }
        UpdatReverseButtonStatus(moves.Count > 0);
    }

    public void AddMove(Move move)
    {
        moves.Push(move);
        UpdatReverseButtonStatus(true);
    }

    public void ResetMoves()
    {
        moves.Clear();
        UpdatReverseButtonStatus(false);
    }
    public void UpdatReverseButtonStatus(bool interactable)
    {
        ReverseButton.interactable = interactable;
    }
}
