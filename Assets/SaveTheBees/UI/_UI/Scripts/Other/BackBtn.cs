using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackBtn : MonoBehaviour
{
    [SerializeField] private Button btn;
    [SerializeField] private BackType EbackType;

    private void Start()
    {
        btn.onClick.AddListener(Back);
    }

    private void Back()
    {
        SoundFXMNG.Ins.PlaySFX(SoundFXMNG.Ins.click);
        switch (EbackType)
        {
            case BackType.T1:
                //Back khi choi (chua win)
                LevelManagerBeeGame.Ins.DespawnMap();
                UIManagerBeeGame.Ins.CloseUI<InGameCanvas>();
 
                UIManagerBeeGame.Ins.OpenUI<ChangeSceneCanvas>();
                Observer.Notify("Wait", 2f, new Action(ChangeScene));  
                break;
            case BackType.T2:
                //Back khi dang chon level
                UIManagerBeeGame.Ins.CloseUI<SelectLevelCanvas>();
                UIManagerBeeGame.Ins.OpenUI<ChangeSceneCanvas>();
                Observer.Notify("Wait", 2f, new Action(ChangeScene));
                break;
            case BackType.T3:
                //Back khi win
                LevelManagerBeeGame.Ins.DespawnMap();
                UIManagerBeeGame.Ins.CloseUI<InGameCanvas>();
                UIManagerBeeGame.Ins.CloseUI<WinCanvas>();
                UIManagerBeeGame.Ins.OpenUI<ChangeSceneCanvas>();
                Observer.Notify("Wait", 2f, new Action(ChangeScene));
                break;
            case BackType.T4:
                //Load Shop
                UIManagerBeeGame.Ins.CloseUI<ShopCanvas>();
                UIManagerBeeGame.Ins.OpenUI<ChangeSceneCanvas>();
                Observer.Notify("Wait", 2f, new Action(ChangeScene));
                break;
        }
    }

    private void ChangeScene()
    {
        UIManagerBeeGame.Ins.OpenUI<MainMenuCanvas>();
        UIManagerBeeGame.Ins.CloseUI<ChangeSceneCanvas>();
    }
}

public enum BackType
{
    T1 = 0,
    T2 = 1,
    T3 = 2,
    T4 = 3
}
