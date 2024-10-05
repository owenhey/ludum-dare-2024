using System.Collections.Generic;
using UnityEngine;

public class RenderWord : MonoBehaviour {
    public float SpaceBetween = .5f;
    public List<RenderToken> TokenElements;

    private void Start() {
        ShowWord(new int[]{4, 13, 23, 27});
    }
    
    public void ShowWord(int[] tokens) {
        float xStart = -((tokens.Length - 1) / 4);
        for (int i = 0; i < Mathf.Max(tokens.Length, TokenElements.Count); i++) {
            bool show = i < tokens.Length;
            TokenElements[i].gameObject.SetActive(show);
            if (!show) return;

            TokenElements[i].transform.localPosition = new Vector3(xStart + SpaceBetween * i, 0, 0);
            TokenElements[i].ShowLetter(tokens[i]);
        }
    }

    private void EnsureEnoughElements(int amount) {
        while (TokenElements.Count < amount) {
            RenderToken newToken = Instantiate(TokenElements[0]);
            TokenElements.Add(newToken);
        }
    }
}
