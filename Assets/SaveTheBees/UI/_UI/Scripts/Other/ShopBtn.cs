using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopBtn : MonoBehaviour
{
    [SerializeField] private Button btn;
    private void Start()
    {
        btn.onClick.AddListener(OpenShop);
    }

    private void OpenShop()
    {
        SoundFXMNG.Ins.PlaySFX(SoundFXMNG.Ins.click);
        UIManagerBeeGame.Ins.CloseUI<MainMenuCanvas>();
        UIManagerBeeGame.Ins.OpenUI<ChangeSceneCanvas>();
        Observer.Notify("Wait", 2f, new Action(ChangeScene));
    }

    private void ChangeScene()
    {
        UIManagerBeeGame.Ins.OpenUI<ShopCanvas>();
        UIManagerBeeGame.Ins.CloseUI<ChangeSceneCanvas>();
    }
}
