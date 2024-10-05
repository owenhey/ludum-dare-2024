using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Creature : MonoBehaviour {
    public RenderToken token;

    public static Creature Nearest;

    private string selectedWord;
    
    private void Start() {
        char ran = (char)('a' + Random.Range(0, 25));
        Debug.Log("RAndom char: " + ran);
        token.ShowLetter((char)('a' + Random.Range(0, 25)));
    }

    public void InteractWith() {
        CallRenderWordToMe();
        SelectWord();
    }

    private void SelectWord() {
        selectedWord = "apple";
        RenderWord.Instance.ShowWord(selectedWord);
    }

    private void CallRenderWordToMe() {
        Transform renderTransform = RenderWord.Instance.transform;
        renderTransform.gameObject.SetActive(true);
        renderTransform.transform.parent = transform;
        renderTransform.transform.position = transform.position + Vector3.up;
    }

    public void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            Nearest = this;

            CallRenderWordToMe();
            RenderWord.Instance.ShowWord("space");
        }
    }
    
    public void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            if (Nearest == this) {
                Transform renderTransform = RenderWord.Instance.transform;
                renderTransform.parent = null;
                renderTransform.gameObject.SetActive(false);
                Nearest = null;
            }

        }
    }
}
