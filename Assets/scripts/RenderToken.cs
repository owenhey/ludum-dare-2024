using System;
using UnityEngine;
using DG.Tweening;

public class RenderToken : MonoBehaviour {
    private bool _copiedMat;
    
    public MeshRenderer TokenMR;
    private Material _tokenMat;

    private float _delay = 0;

    public static Color DefaultColor = new Color(.1f, .1f, .1f);

    public void ShowLetter(char c, float delay = 0) {
        _delay = delay;
        if (!_copiedMat) {
            _tokenMat = new Material(TokenMR.sharedMaterial);
            TokenMR.material = _tokenMat;
        }
        _tokenMat.SetFloat("_letter", GetIndexFromLetter(c) + .1f);
    }

    public void ShowColor(Color c) {
        _tokenMat.SetColor("_color", c);
    }

    private int GetIndexFromLetter(char c) {
        if (c >= 'a' && c <= 'z') {
            return c - 'a';
        }

        if (c == '!') return 26;
        if (c == ' ') return 27;

        throw new ArgumentException("char inputted was not valid");
    }
}
