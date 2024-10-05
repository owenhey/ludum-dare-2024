using System;
using UnityEngine;
using DG.Tweening;

public class RenderToken : MonoBehaviour {
    private bool _copiedMat;
    
    public MeshRenderer TokenMR;
    private Material _tokenMat;

    private float _delay = 0;

    private void OnEnable() {
        transform.DOKill();
        transform.DOScale(.03f, .1f).From(0).SetDelay(_delay);
    }

    public void ShowLetter(int index, float delay = 0) {
        _delay = delay;
        if (!_copiedMat) {
            _tokenMat = new Material(TokenMR.sharedMaterial);
            TokenMR.material = _tokenMat;
        }
        _tokenMat.SetFloat("_letter", index + .1f);
    }

    public void ShowColor(Color c) {
        _tokenMat.SetColor("_color", c);
    }
}
