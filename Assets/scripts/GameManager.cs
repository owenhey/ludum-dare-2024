using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public Movement movement;
    public Creature CreaturePrefab;

    public Transform CreatureTargetParent;

    public EndGameScreen EndGameScreen;
    
    private bool typing = false;

    private RenderWord availableWord;
    private RenderWord typedWord;

    private List<Creature> creatures;

    public static int LettersNotUsed = 0;

    public void StartGame() {
        Movement.Interacting = true;
        typing = false;
        Fader.instance.FadeWithFunction(StartGameLogic);
        Creature.SentAwayThisRound = false;
    }

    private void StartGameLogic() {
        Movement.Interacting = false;
        
        movement.Warp(Vector3.zero);
        movement.CreaturesFollowing.Clear();
        
        // Make some creatures to start the game
        int numCreatures = 2;
        for (int i = 0; i < numCreatures; i++) {
            var newcreature = Instantiate(CreaturePrefab, -Vector3.forward * (i + 1), quaternion.identity);
            newcreature.FollowPlayer();
        }
    }

    public void FinishCollection() {
        Movement.Interacting = true;
        typing = false;
        Fader.instance.FadeWithFunction(SwitchToMakingWord);
    }

    private void SwitchToMakingWord() {
        typing = false;
        movement.Warp(Vector3.zero);
        LettersNotUsed = 0;

        var creaturesFollowing = movement.CreaturesFollowing;

        int childCount = CreatureTargetParent.childCount;
        for (int i = childCount - 1; i >= 0; i--) {
            Destroy(CreatureTargetParent.GetChild(i).gameObject);
        }

        float dTheta = (2 * Mathf.PI) / creaturesFollowing.Count;
        for (int i = 0; i < creaturesFollowing.Count; i++) {
            var newGO = new GameObject();
            newGO.transform.parent = CreatureTargetParent;
            Vector3 position = new Vector3(Mathf.Sin(i * dTheta), 0, Mathf.Cos(i * dTheta)) * 1.5f;
            newGO.transform.position = position;
            
            creaturesFollowing[i].FollowTarget = newGO.transform;
            creaturesFollowing[i].Warp(position);
        }

        StartCoroutine(EndAnimation(creaturesFollowing));
    }

    private IEnumerator EndAnimation(List<Creature> creatures) {
        this.creatures = creatures;
        yield return new WaitForSeconds(2.5f);
        string word = "";

        availableWord = RenderWordsPool.Get();
        availableWord.gameObject.SetActive(true);
        availableWord.transform.position = Vector3.up * 1.5f;
        
        for (int i = 0; i < creatures.Count; i++) {
            char letter = creatures[i].Char;
            creatures[i].RemoveLetter();
            word += letter;
            availableWord.ShowWord(word);
            yield return new WaitForSeconds(.5f);
        }

        availableLetters = word;
        collectedLetters = word;
        
        yield return new WaitForSeconds(.5f);
        
        typedWord = RenderWordsPool.Get();
        typedWord.gameObject.SetActive(true);
        typedWord.transform.position = Vector3.up * 2.5f;

        typing = true;
        currentEntered = "";
        
        RefreshShowing();
    }

    private string currentEntered = "";
    private string availableLetters = "";
    private string collectedLetters = "";
    private void Update() {
        if (!typing) return;
        
        char entered = '!';
        if (Input.GetKeyDown(KeyCode.A)) entered = 'a';
        else if (Input.GetKeyDown(KeyCode.B)) entered = 'b';
        else if (Input.GetKeyDown(KeyCode.C)) entered = 'c';
        else if (Input.GetKeyDown(KeyCode.D)) entered = 'd';
        else if (Input.GetKeyDown(KeyCode.E)) entered = 'e';
        else if (Input.GetKeyDown(KeyCode.F)) entered = 'f';
        else if (Input.GetKeyDown(KeyCode.G)) entered = 'g';
        else if (Input.GetKeyDown(KeyCode.H)) entered = 'h';
        else if (Input.GetKeyDown(KeyCode.I)) entered = 'i';
        else if (Input.GetKeyDown(KeyCode.J)) entered = 'j';
        else if (Input.GetKeyDown(KeyCode.K)) entered = 'k';
        else if (Input.GetKeyDown(KeyCode.L)) entered = 'l';
        else if (Input.GetKeyDown(KeyCode.M)) entered = 'm';
        else if (Input.GetKeyDown(KeyCode.N)) entered = 'n';
        else if (Input.GetKeyDown(KeyCode.O)) entered = 'o';
        else if (Input.GetKeyDown(KeyCode.P)) entered = 'p';
        else if (Input.GetKeyDown(KeyCode.Q)) entered = 'q';
        else if (Input.GetKeyDown(KeyCode.R)) entered = 'r';
        else if (Input.GetKeyDown(KeyCode.S)) entered = 's';
        else if (Input.GetKeyDown(KeyCode.T)) entered = 't';
        else if (Input.GetKeyDown(KeyCode.U)) entered = 'u';
        else if (Input.GetKeyDown(KeyCode.V)) entered = 'v';
        else if (Input.GetKeyDown(KeyCode.W)) entered = 'w';
        else if (Input.GetKeyDown(KeyCode.X)) entered = 'x';
        else if (Input.GetKeyDown(KeyCode.Y)) entered = 'y';
        else if (Input.GetKeyDown(KeyCode.Z)) entered = 'z';
        else if (Input.GetKeyDown(KeyCode.Backspace)) entered = 'B';
        else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) entered = 'E';

        if (entered == '!') return;
        if (entered == 'B') {
            Debug.Log($"Trying to delete! {currentEntered}");
            if (currentEntered.Length > 0) {
                availableLetters += currentEntered[^1];
                currentEntered = currentEntered.Substring(0, currentEntered.Length - 1);
                RefreshShowing();
                return;
            }
            return;
        }

        if (entered == 'E') {
            if (currentEntered.Length != collectedLetters.Length) {
                availableWord.Shake();
                return;
            }
            
            bool isWord = Words.Instance.GetRarity(currentEntered) != -1;
            if (!isWord) {
                availableWord.Shake();
                return;
            }

            typing = false;

            StartCoroutine(EndGame());
            
            return;
        }

        if (!availableLetters.Contains(entered)) {
            availableWord.Shake();
            return;
        }

        currentEntered += entered;
        availableLetters = availableLetters.Remove(availableLetters.IndexOf(entered), 1);
        
        Debug.Log($"available letters: {availableLetters}");
        
        RefreshShowing();
    }

    private void RefreshShowing() {
        if (currentEntered.Length == 0) {
            typedWord.ShowWord("type your word!");
            availableWord.ShowWord(collectedLetters, false);
            return;
        }
        typedWord.ShowWord(currentEntered);

        if (currentEntered.Length == collectedLetters.Length) {
            bool isWord = Words.Instance.GetRarity(currentEntered) != -1;
            if (isWord) {
                availableWord.ShowWord("press enter!", false);
            }
            else {
                availableWord.ShowWord("word not known", false);
            }
            return;
        }

        List<TokenData> tokens = new();
        
        string availableCopy = availableLetters;
        for (int i = 0; i < collectedLetters.Length; i++) {
            char letter = collectedLetters[i];
            bool stillHas = availableCopy.Contains(letter);
            if (stillHas) {
                availableCopy = availableCopy.Remove(availableCopy.IndexOf(letter), 1);
                tokens.Add(new TokenData(letter, RenderToken.DefaultColor, false));
            }
        }
        
        availableWord.ShowWord(tokens.ToArray());
    }

    private IEnumerator EndGame() {
        LettersNotUsed = collectedLetters.Length - currentEntered.Length;
        availableWord.gameObject.SetActive(false);
        // make all the creatures hop
        for (int i = 0; i < creatures.Count; i++) {
            yield return new WaitForSeconds(.25f);
            creatures[i].LitteHop();
        }
        
        // Go to scoring
        yield return new WaitForSeconds(1.0f);
        EndGameScreen.EndGame(currentEntered);
    }
}
