using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGameBtn : MonoBehaviour
{
    [SerializeField] private Button startBtn;

    private void Start()
    {
        startBtn.onClick.AddListener(LoadCurMap);
    }

    public void LoadCurMap()
    {
        SoundFXMNG.Ins.PlaySFX(SoundFXMNG.Ins.click);
        UIManagerBeeGame.Ins.CloseUI<MainMenuCanvas>();
        UIManagerBeeGame.Ins.OpenUI<ChangeSceneCanvas>();
        Observer.Notify("Wait", 2f, new Action(ChangeScene));
    }

    private void ChangeScene()
    {
        LevelManagerBeeGame.Ins.LoadMapByID(LevelManagerBeeGame.Ins.curMap);
        UIManagerBeeGame.Ins.OpenUI<InGameCanvas>().OnIniT();
        UIManagerBeeGame.Ins.CloseUI<ChangeSceneCanvas>();
    }
}
