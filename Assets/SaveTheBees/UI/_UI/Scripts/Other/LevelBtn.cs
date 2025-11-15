using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelBtn : MonoBehaviour
{
    public int id;
    public Image img;
    public Sprite[] spr;
    public Text txt;
    public Button btn;

    private void Start()
    {
        btn.onClick.AddListener(ChangeScene);
    }

    public void ChangeScene()
    {
        SoundFXMNG.Ins.PlaySFX(SoundFXMNG.Ins.click);
        UIManagerBeeGame.Ins.CloseUI<SelectLevelCanvas>();
        UIManagerBeeGame.Ins.OpenUI<ChangeSceneCanvas>();
        Observer.Notify("Wait", 2f, new Action(LoadLevel));
    }

    private void LoadLevel()
    {
        LevelManagerBeeGame.Ins.LoadMapByID(id);
        UIManagerBeeGame.Ins.OpenUI<InGameCanvas>().OnIniT();
        UIManagerBeeGame.Ins.CloseUI<ChangeSceneCanvas>();
    }
}
