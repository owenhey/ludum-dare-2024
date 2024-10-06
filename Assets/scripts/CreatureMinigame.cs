using System.Collections;
using UnityEngine;

public class CreatureMinigame : MonoBehaviour {
    public Creature Creature;

    private string currentWord;
    private int currentIndex;

    public Color GoodColor;
    public Color BadColor;

    private RenderWord _renderWord;

    public void StartMinigame(RenderWord renderWord) {
        _renderWord = renderWord;
        Movement.Interacting = true;
        StartCoroutine(Minigame());
    }

    private IEnumerator Minigame() {
        int numWords = 3;
        for (int i = 0; i < numWords; i++) {
            currentWord = GenerateWord();
            currentIndex = 0;
            
            _renderWord.ShowWord(currentWord, BadColor);

            yield return new WaitForSeconds(.1f);
            yield return new WaitUntil(Finished);
        }
        
        WinMinigame();
    }

    private void LookForInput() {
        if (string.IsNullOrEmpty(currentWord)) return;
        if (currentIndex >= currentWord.Length) return;
        
        char c = '!';
        if (Input.GetKeyDown(KeyCode.A)) c = 'a';
        else if (Input.GetKeyDown(KeyCode.B)) c = 'b';
        else if (Input.GetKeyDown(KeyCode.C)) c = 'c';
        else if (Input.GetKeyDown(KeyCode.D)) c = 'd';
        else if (Input.GetKeyDown(KeyCode.E)) c = 'e';
        else if (Input.GetKeyDown(KeyCode.F)) c = 'f';
        else if (Input.GetKeyDown(KeyCode.G)) c = 'g';
        else if (Input.GetKeyDown(KeyCode.H)) c = 'h';
        else if (Input.GetKeyDown(KeyCode.I)) c = 'i';
        else if (Input.GetKeyDown(KeyCode.J)) c = 'j';
        else if (Input.GetKeyDown(KeyCode.K)) c = 'k';
        else if (Input.GetKeyDown(KeyCode.L)) c = 'l';
        else if (Input.GetKeyDown(KeyCode.M)) c = 'm';
        else if (Input.GetKeyDown(KeyCode.N)) c = 'n';
        else if (Input.GetKeyDown(KeyCode.O)) c = 'o';
        else if (Input.GetKeyDown(KeyCode.P)) c = 'p';
        else if (Input.GetKeyDown(KeyCode.Q)) c = 'q';
        else if (Input.GetKeyDown(KeyCode.R)) c = 'r';
        else if (Input.GetKeyDown(KeyCode.S)) c = 's';
        else if (Input.GetKeyDown(KeyCode.T)) c = 't';
        else if (Input.GetKeyDown(KeyCode.U)) c = 'u';
        else if (Input.GetKeyDown(KeyCode.V)) c = 'v';
        else if (Input.GetKeyDown(KeyCode.W)) c = 'w';
        else if (Input.GetKeyDown(KeyCode.X)) c = 'x';
        else if (Input.GetKeyDown(KeyCode.Y)) c = 'y';
        else if (Input.GetKeyDown(KeyCode.Z)) c = 'z';
        
        if (c == '!') return;

        char targetChar = currentWord[currentIndex];
        if (targetChar != c) {
            WrongInput();
            return;
        }
        else {
            currentIndex++;
            UpdateWithProgress();
        }
    }

    private void UpdateWithProgress() {
        TokenData[] tokens = new TokenData[currentWord.Length];
        for (int i = 0; i < tokens.Length; i++) {
            bool gotToLetterYet = i < currentIndex;
            tokens[i] = new TokenData(currentWord[i], gotToLetterYet ? GoodColor : BadColor, false);
        }
        _renderWord.ShowWord(tokens);
    }

    private void WrongInput() {
        currentIndex = 0;
        _renderWord.Shake();
        UpdateWithProgress();
    }

    private bool Finished() {
        return currentIndex >= currentWord.Length;
    }

    private string GenerateWord() {
        return Words.Instance.GetRandom(3000, 4, 8, Creature.Char);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            CancelMinigame();
        }

        LookForInput();
    }

    private void WinMinigame() {
        enabled = false;
        Movement.Interacting = false;
        _renderWord.gameObject.SetActive(false);
        Creature.LitteHop();
        StopAllCoroutines();

        Creature.FollowPlayer();
    }

    public void CancelMinigame() {
        enabled = false;
        StopAllCoroutines();
        Movement.Interacting = false;
        _renderWord.gameObject.SetActive(false);
        Creature.lookingForPlayer = true;
        Creature.ForceNearest();
    }
}
