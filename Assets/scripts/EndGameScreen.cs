using System;
using System.Collections;
using DefaultNamespace;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGameScreen : MonoBehaviour {
    public GameObject content;
    public CanvasGroup Cg;

    public TextMeshProUGUI wordField;
    public Button playNextButton;
    
    public IncrementingScore lengthScore;
    public IncrementingScore rarityScore;
    public IncrementingScore scrabbleScore;
    public IncrementingScore consonentsScore;
    public IncrementingScore uniqueLettersScore;
    public IncrementingScore keepAllLettersScore;
    public IncrementingScore palindromeScore;
    public IncrementingScore useAllLettersScore;
    public IncrementingScore totalScore;

    private void Awake() {
        playNextButton.onClick.AddListener(PlayAgain);
    }

    private void PlayAgain() {
        playNextButton.interactable = false;
        Sound.I.PlayKnock2();
        Sound.I.StopCountUp();
        
        Fader.instance.FadeWithFunction(GoToTitle);
    }

    private void GoToTitle() {
        GameManager.Instance.GoTitle();
        content.gameObject.SetActive(false);
        Cg.alpha = 0;
    }
    
    public void EndGame(string word) {
        StartCoroutine(EndGameC(word));
    }

    private IEnumerator EndGameC(string word) {
        playNextButton.interactable = false;
        wordField.text = word;
        content.gameObject.SetActive(true);
        Cg.DOFade(1.0f, 1.0f).From(0);

        ScoreData score = Scorer.Score(word);

        yield return new WaitForSeconds(.6f);
        Sound.I.StartCountUp();
        lengthScore.CountUpTo(score.lengthScore);
        yield return new WaitForSeconds(.6f);
        rarityScore.CountUpTo(score.rarityScore);
        yield return new WaitForSeconds(.6f);
        scrabbleScore.CountUpTo(score.scrabbleScore);
        yield return new WaitForSeconds(.6f);
        consonentsScore.CountUpTo(score.consonentsScore);
        yield return new WaitForSeconds(.6f);
        uniqueLettersScore.CountUpTo(score.uniqueLettersScore);
        yield return new WaitForSeconds(.6f);
        keepAllLettersScore.CountUpTo(score.keepAllLettersScore);
        yield return new WaitForSeconds(.6f);
        palindromeScore.CountUpTo(score.palindromeScore);
        yield return new WaitForSeconds(.6f);
        useAllLettersScore.CountUpTo(score.useAllLettersScore);
        yield return new WaitForSeconds(.6f);
        totalScore.CountUpTo(score.total);
        yield return new WaitForSeconds(1.5f);
        Sound.I.StopCountUp();

        playNextButton.interactable = true;
    }
}
