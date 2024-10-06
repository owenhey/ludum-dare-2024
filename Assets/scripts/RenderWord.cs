using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public struct TokenData {
    public char C;
    public Color Color;
    public bool Animate;

    public TokenData(char c, Color color, bool animate) {
        C = c;
        Color = color;
        Animate = animate;
    }
}

public class RenderWord : MonoBehaviour {
    public float SpaceBetween = .5f;
    public List<RenderToken> TokenElements;
    public SpriteRenderer SR;
    public Transform Content;

    private Transform FollowTrans;
    private Vector3 offset;


    public void Shake() {
        Content.DOKill();
        Content.DOShakePosition(.3f, .1f, 30);
    }

    public void SetFollow(Transform t, Vector3 o) {
        FollowTrans = t;
        offset = o;
        if (t != null) transform.position = FollowTrans.position + o;
    }

    private void LateUpdate() {
        if (FollowTrans) {
            transform.position = FollowTrans.position + offset;
        }
    }

    private void OnDisable() {
        FollowTrans = null;
    }

    public void ShowWord(string word) {
        List<TokenData> tokens = new List<TokenData>(word.Length);
        for (int i = 0; i < word.Length; i++) {
            tokens.Add(new TokenData(word[i], RenderToken.DefaultColor, true));
        }
        ShowWord(tokens.ToArray());
    }
    
    public void ShowWord(string word, bool animate) {
        List<TokenData> tokens = new List<TokenData>(word.Length);
        for (int i = 0; i < word.Length; i++) {
            tokens.Add(new TokenData(word[i], RenderToken.DefaultColor, animate));
        }
        ShowWord(tokens.ToArray());
    }
    
    public void ShowWord(string word, Color c) {
        List<TokenData> tokens = new List<TokenData>(word.Length);
        for (int i = 0; i < word.Length; i++) {
            tokens.Add(new TokenData(word[i], c, true));
        }
        ShowWord(tokens.ToArray());
    }

    public void ShowWord(TokenData[] tokens) {
        EnsureEnoughElements(tokens.Length);
        
        float xStart = ((tokens.Length - 1) / 2.0f) * SpaceBetween;
        for (int i = 0; i < Mathf.Max(tokens.Length, TokenElements.Count); i++) {
            bool show = i < tokens.Length;
            TokenElements[i].gameObject.SetActive(show);
            TokenElements[i].transform.DOKill();
            if (!show) continue;

            TokenElements[i].transform.localPosition = new Vector3(xStart - (SpaceBetween * i), 0, 0);
            TokenElements[i].ShowLetter(tokens[i].C);
            TokenElements[i].ShowColor(tokens[i].Color);
            TokenElements[i].transform.localScale = Vector3.one * .03f;
            if(tokens[i].Animate)
                TokenElements[i].transform.DOScale(.03f, .1f).SetDelay((i + 1) * .05f).From(0);
        }

        SR.size = new Vector2(SpaceBetween * tokens.Length + .5f, SR.size.y);
    }

    private void EnsureEnoughElements(int amount) {
        while (TokenElements.Count < amount) {
            RenderToken newToken = Instantiate(TokenElements[0], TokenElements[0].transform.parent);
            TokenElements.Add(newToken);
        }
    }
}
