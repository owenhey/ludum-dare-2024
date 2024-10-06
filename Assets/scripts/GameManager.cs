using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour {

    public Movement movement;
    public Creature CreaturePrefab;
    public CreatureSpawner CreatureSpawner;

    public Transform CreatureTargetParent;

    public EndGameScreen EndGameScreen;
    
    private bool typing = false;

    public GameObject gameUI;
    public GameObject wordSelectUI;
    public GameObject titleUI;

    public PanningCams panning;
    
    private RenderWord availableWord;
    private RenderWord typedWord;
    private RenderWord enter;

    public TextMeshProUGUI timerText;
    private float StartTime;

    private bool timing = false;

    public static GameManager Instance;

    private List<Creature> creatures;

    public static int LettersNotUsed = 0;

    public static string GAMETYPE = "FREEPLAY";

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        Application.targetFrameRate = 60;
        GoTitle();
    }

    public void GoTitle() {
        Movement.Interacting = true;
        
        gameUI.gameObject.SetActive(false);
        wordSelectUI.gameObject.SetActive(false);
        titleUI.gameObject.SetActive(true);
        
        CreatureSpawner.DestroyAllCreatures();
        CreatureSpawner.Spawn();
        panning.gameObject.SetActive(true);
    }
    
    public void StartGame(int type) {
        if (type == 0) {
            GAMETYPE = "TIMED";
        }
        else {
            GAMETYPE = "FREEPLAY";
        }
        
        Movement.Interacting = true;
        typing = false;
        Fader.instance.FadeWithFunction(StartGameLogic);
        Creature.SentAwayThisRound = false;
    }

    private void StartGameLogic() {
        Movement.Interacting = false;
        CreatureSpawner.DestroyAllCreatures();
        movement.Warp(Vector3.zero);
        movement.CreaturesFollowing.Clear();
        panning.gameObject.SetActive(false);
        timing = true;

        timing = GAMETYPE == "TIMED";
        if (timing) {
            StartTime = Time.time;
        }
        timerText.gameObject.SetActive(timing);
        
        gameUI.gameObject.SetActive(true);
        wordSelectUI.gameObject.SetActive(false);
        titleUI.gameObject.SetActive(false);
        
        // Make some creatures to start the game
        int numCreatures = 2;
        for (int i = 0; i < numCreatures; i++) {
            var newcreature = Instantiate(CreaturePrefab, -Vector3.forward * (i + 1), quaternion.identity);
            newcreature.FollowPlayer();
            newcreature.genType = i == 0 ? CharGenType.Vowel : CharGenType.Consonant;
            newcreature.CanRelease = false;
            CreatureSpawner.allCreatures.Add(newcreature);
            StartCoroutine(JumpAfterDelay(newcreature, 1.00f + .15f * i));
        }
        
        CreatureSpawner.Spawn();
    }

    private IEnumerator JumpAfterDelay(Creature c, float f) {
        yield return new WaitForSeconds(f);
        c.LitteHop();
    }

    public void FinishCollection() {
        if (movement.CreaturesFollowing.Count < 4) {
            movement.ShowOverhead("need four!");
            return;
        }

        timing = false;
        Movement.Interacting = true;
        typing = false;
        Fader.instance.FadeWithFunction(SwitchToMakingWord);
    }

    private void SwitchToMakingWord() {
        CreatureSpawner.DestroyUncollectedCreatures();
        typing = false;
        movement.Warp(Vector3.zero);
        LettersNotUsed = 0;
        
        gameUI.gameObject.SetActive(false);
        wordSelectUI.gameObject.SetActive(true);
        titleUI.gameObject.SetActive(false);

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

        enter = RenderWordsPool.Get();
        enter.transform.position = Vector3.up * 10;
        enter.ShowWord("enter", false);
        enter.gameObject.SetActive(true);

        typing = true;
        currentEntered = "";
        
        RefreshShowing();
    }

    private IEnumerator FailedC() {
        movement.ShowOverhead("needed four!");
        gameUI.gameObject.SetActive(false);
        yield return new WaitForSeconds(2.0f);
        Fader.instance.FadeWithFunction(GoTitle);
    }

    private string currentEntered = "";
    private string availableLetters = "";
    private string collectedLetters = "";
    private void Update() {
        if (timing) {
            int totalSeconds = 120;
            int secondsElapsed = (int)(Time.time - StartTime);
            int secondsLeft = Mathf.Max(0, totalSeconds - secondsElapsed);
            
            int minutes = secondsLeft / 60;
            int remainingSeconds = secondsLeft % 60;
    
            timerText.text = string.Format("{0}:{1:00}", minutes, remainingSeconds);
            
            if (secondsElapsed > totalSeconds) {
                timing = false;
                Movement.Interacting = true;
                if (movement.CreaturesFollowing.Count < 4) {
                    StartCoroutine(FailedC());
                    return;
                }
                else {
                    FinishCollection();
                }
                return;
            }
        }
        
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
            if (currentEntered.Length < 4) {
                availableWord.Shake();
                return;
            }
            
            bool isWord = Words.Instance.GetRarity(currentEntered) != -1;
            if (!isWord) {
                typedWord.Shake();
                return;
            }

            typing = false;
            LettersNotUsed = availableLetters.Length;
            

            StartCoroutine(EndGame());
            
            return;
        }

        if (!availableLetters.Contains(entered)) {
            availableWord.Shake();
            return;
        }

        currentEntered += entered;
        availableLetters = availableLetters.Remove(availableLetters.IndexOf(entered), 1);
        
        RefreshShowing();
    }

    private void RefreshShowing() {
        if (currentEntered.Length == 0) {
            typedWord.ShowWord("type your word!");
            availableWord.ShowWord(collectedLetters, false);
            return;
        }
        typedWord.ShowWord(currentEntered);

        bool showEnter = currentEntered.Length >= 4;
        Vector3 showPos = typedWord.GetMostRightPos() + Camera.main.transform.right;
        enter.transform.position = showEnter ? showPos : Vector3.up * 10;
        enter.ShowWord("enter", false);

        if (currentEntered.Length == collectedLetters.Length) {
            bool isWord = Words.Instance.GetRarity(currentEntered) != -1;
            if (isWord) {
                availableWord.ShowWord("", false);
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

    public void GiveUp() {
        if (!typing) return;
        typing = false;

        Fader.instance.FadeWithFunction(GoTitle);
    }

    public void GiveUpDuringGame() {
        Movement.Interacting = true;
        gameUI.SetActive(false);
        timing = false;
        Fader.instance.FadeWithFunction(GoTitle);
    }

    private IEnumerator EndGame() {
        LettersNotUsed = collectedLetters.Length - currentEntered.Length;
        availableWord.gameObject.SetActive(false);
        enter.gameObject.SetActive(false);
        
        wordSelectUI.SetActive(false);
        
        // make all the creatures hop
        for (int i = 0; i < creatures.Count; i++) {
            yield return new WaitForSeconds(Random.Range(.05f, .15f));
            creatures[i].LitteHop();
        }
        
        // Go to scoring
        yield return new WaitForSeconds(1.0f);
        EndGameScreen.EndGame(currentEntered);
    }
}
