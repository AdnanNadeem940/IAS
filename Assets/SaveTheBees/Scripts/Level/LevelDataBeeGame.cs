using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelDataBeeGame : GameUnit
{
    public int id;
    public PlayerCtrl playerCtrl;
    public List<Beehive> ListBeehive = new List<Beehive>();
    public Transform pos;
    public ELevel eLevl;

    [SerializeField] private Transform child;

    private void OnEnable()
    {
        playerCtrl.SetSprAsset(LevelManagerBeeGame.Ins.id);

        //Debug.Log("Transform player to pos");
        playerCtrl.transform.position = pos.position;
        playerCtrl.transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    private void Update()
    {
        if (LevelManagerBeeGame.Ins.timesUp)
        {
            // So sánh và cập nhật trạng thái thắng của map
            if (eLevl == LevelManagerBeeGame.Ins.mapSO.mapList[LevelManagerBeeGame.Ins.curMap].eLevel &&
                !LevelManagerBeeGame.Ins.mapSO.mapList[LevelManagerBeeGame.Ins.curMap].isWon)
            {
                LevelManagerBeeGame.Ins.mapSO.mapList[LevelManagerBeeGame.Ins.curMap].isWon = true;
                SaveWinState(LevelManagerBeeGame.Ins.curMap);
                Debug.Log("Map " + LevelManagerBeeGame.Ins.curMap + " is won.");
                LevelManagerBeeGame.Ins.curMap++;
            }

            SetCurMap();
        }
    }

    private void SetCurMap()
    {
        PlayerPrefs.SetInt("CurrentMap", LevelManagerBeeGame.Ins.curMap);
        PlayerPrefs.Save();
    }

    private void SaveWinState(int mapIndex)
    {
        string key = "MapWin_" + mapIndex;
        PlayerPrefs.SetInt(key, 1); // Lưu lại trạng thái thắng của map
        PlayerPrefs.Save();
        LevelManagerBeeGame.Ins.mapSO.LoadWinStates();
    }

    private void OnDrawGizmosSelected()
    {
        ListBeehive.Clear();
        for (int i = 0; i < child.transform.childCount; i++)
        {
            Beehive b = child.transform.GetChild(i).GetComponent<Beehive>();
            if (b != null)
            {
                ListBeehive.Add(b);
            }
        }
    }
}
