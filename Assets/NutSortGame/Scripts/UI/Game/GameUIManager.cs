using TMPro;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
	[SerializeField] private GameObject winMenu;
	[SerializeField] private GameObject loseMenu;
	[SerializeField] private GameObject VictoryUI;

	[SerializeField] private TMP_Text moveTimesText;

	private void Awake()
	{
        GameManagerNS.Instance.OnGameWin.AddListener(OpenWinMenu);
        GameManagerNS.Instance.OnGameLose.AddListener(OpenLoseMenu);
        GameManagerNS.Instance.OnMoveTimesChanged.AddListener(ChangeMoveTimes);
	}

	private void OpenWinMenu()
	{
		//len nhac

		winMenu.SetActive(true);
	}

	private void OpenLoseMenu()
	{
		//len nhac

		loseMenu.SetActive(true);
	}

	public void CloseAll()
	{
		winMenu.SetActive(false);
		loseMenu.SetActive(false);
		VictoryUI.SetActive(false);
	}

	public void Victory()
	{
		VictoryUI.SetActive(true);
	}

	private void ChangeMoveTimes(int times)
	{
		moveTimesText.text = $"YOUR MOVES: {times}";
	}
}