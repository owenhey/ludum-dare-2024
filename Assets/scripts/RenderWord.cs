using System.Collections.Generic;
using UnityEngine;

public class RenderWord : MonoBehaviour {
    public float SpaceBetween = .5f;
    public List<RenderToken> TokenElements;
    public SpriteRenderer SR;

    public static RenderWord Instance;

    public void Awake() {
        Instance = this;
    }
    
    private void Start() {
        ShowWord(new int[]{0, 1, 2, 3});
    }

    public void ShowWord(string word) {
        List<int> ints = new List<int>(word.Length);
        for (int i = 0; i < word.Length; i++) {
            ints.Add(word[i] - 'a');
        }
        ShowWord(ints.ToArray());
    }
    
    public void ShowWord(int[] tokens) {
        EnsureEnoughElements(tokens.Length);
        
        float xStart = ((tokens.Length - 1) / 2.0f) * SpaceBetween;
        for (int i = 0; i < Mathf.Max(tokens.Length, TokenElements.Count); i++) {
            bool show = i < tokens.Length;
            TokenElements[i].gameObject.SetActive(show);
            if (!show) return;

            TokenElements[i].transform.localPosition = new Vector3(xStart - (SpaceBetween * i), 0, 0);
            TokenElements[i].ShowLetter(tokens[i], (i + 1) * .1f);
            TokenElements[i].ShowColor(new Color(.1f, .1f, .1f));
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
