using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Creature : MonoBehaviour {
    public RenderToken token;
    public CreatureMinigame Minigame;

    public static Creature Nearest;

    private string selectedWord;

    public bool lookingForPlayer = true;
    
    private void Start() {
        char ran = (char)('a' + Random.Range(0, 25));
        Debug.Log("Random char: " + ran);
        token.ShowLetter((char)('a' + Random.Range(0, 25)));

        Minigame.enabled = false;
    }

    public void InteractWith() {
        if (Minigame.enabled) return;
        
        Minigame.enabled = true;
        Minigame.StartMinigame();
    }

    private void CallRenderWordToMe() {
        Transform renderTransform = RenderWord.Instance.transform;
        renderTransform.gameObject.SetActive(true);
        renderTransform.transform.position = transform.position + Vector3.up;
    }

    public void OnTriggerEnter(Collider other) {
        if(!lookingForPlayer) return;
        
        if (other.CompareTag("Player")) {
            Nearest = this;

            CallRenderWordToMe();
            RenderWord.Instance.ShowWord("!");
        }
    }
    
    public void OnTriggerExit(Collider other) {
        if(!lookingForPlayer) return;
        
        if (other.CompareTag("Player")) {
            if (Nearest == this) {
                Transform renderTransform = RenderWord.Instance.transform;
                renderTransform.gameObject.SetActive(false);
                Nearest = null;
            }

        }
    }
}
