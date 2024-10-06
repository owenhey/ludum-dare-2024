using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct FadeData {
    public RectTransform RT;
    public Vector2 posShowing;
    public Vector2 posHidden;
    public float Delay;
}

public class Fader : MonoBehaviour {
    public static Fader instance;

    public GameObject content;

    public FadeData[] fades;
    
    private void Awake() {
        instance = this;
    }

    public void FadeWithFunction(Action midfade) {
        IEnumerator func() {
            ShowFade();
            yield return new WaitForSeconds(2.25f);
            midfade?.Invoke();
            HideFade();
        }

        StartCoroutine(func());
    }

    public void ShowFade() {
        for (int i = 0; i < fades.Length; i++) {
            var fade = fades[i];
            fade.RT.DOAnchorPos(fade.posShowing, .3f).SetDelay(fade.Delay).SetEase(Ease.Linear);
        }
    }

    public void HideFade() {
        for (int i = 0; i < fades.Length; i++) {
            var fade = fades[i];
            fade.RT.DOAnchorPos(fade.posHidden, .3f).SetDelay(1.2f - fade.Delay).SetEase(Ease.Linear);
        }
    }
}
