using System;
using UnityEngine;
using UnityEngine.UI;

public class TitleFunctionality : MonoBehaviour {
    public GameObject _main;
    public GameObject _choose;
    public CanvasGroup _CG;

    private void OnEnable() {
        ResetToNormal();
        _CG.interactable = true;
    }

    private void ResetToNormal() {
        _main.SetActive(true);
        _choose.SetActive(false);
    }

    public void Play() {
        _main.SetActive(false);
        _choose.SetActive(true);
    }

    public void Choose(int choice) {
        _CG.interactable = false;
        GameManager.Instance.StartGame(choice);
    }
    
    public void Back() {
        ResetToNormal();
    }
}
