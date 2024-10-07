using System.Linq;

public class Scorer {
    public static ScoreData Score(string word) {
        ScoreData ret = new ScoreData();

        if (ret.lengthScore < 6) {
            ret.lengthScore = word.Length * 9;
        }
        else if (ret.lengthScore < 8) {
            ret.lengthScore = word.Length * 17;
        }
        else {
            ret.lengthScore = word.Length * 21;
        }

        int rarity = Words.Instance.GetRarity(word);
        int rarityScore = 0;
        if (rarity < 100) {
            rarityScore = 0;
        }
        else if (rarity < 1000) {
            rarityScore = (int)Helpers.RemapClamp(rarity, 100, 1000, 10, 30);
        }
        else if (rarity < 3000) {
            rarityScore = (int)Helpers.RemapClamp(rarity, 1000, 3000, 30, 40);
        }
        else if (rarity < 10000) {
            rarityScore = (int)Helpers.RemapClamp(rarity, 3000, 10000, 40, 80);
        }
        else if (rarity < 50000) {
            rarityScore = (int)Helpers.RemapClamp(rarity, 10000, 50000, 80, 125);
        }
        else
            rarityScore = (int)Helpers.RemapClamp(rarity, 50000, 400000, 125, 180);
        
        ret.rarityScore = rarityScore;

        int scrabbleScore = 0;
        foreach (char c in word) {
            scrabbleScore += GetScrabbleScore(c);
        }
        scrabbleScore *= 4;
        ret.scrabbleScore = scrabbleScore;

        int numConsonents = word.Count(IsConsonent);
        float consonentPercentage = (float)numConsonents / (float)word.Length;
        int consonentScore = (int)Helpers.RemapClamp(consonentPercentage, .25f, 1.0f, 0, word.Length * 13);
        ret.consonentsScore = consonentScore;

        var uniqueLetters = word.Distinct().Count();
        float uniquePercentage = (float)uniqueLetters / (float)word.Length;
        int uniqueScore = (int)Helpers.RemapClamp(uniquePercentage, .5f, 1.0f, 0, word.Length * 13);
        ret.uniqueLettersScore = uniqueScore;

        ret.keepAllLettersScore = Creature.SentAwayThisRound ? 0 : 50;
        
        bool palindrome = word.SequenceEqual(word.Reverse());
        ret.palindromeScore = palindrome ? (word.Length * 25) : 0;

        ret.useAllLettersScore = (GameManager.LettersNotUsed * -20) + 25;

        return ret;
    }

    public static bool IsConsonent(char c) {
        if (c == 'a') return false;
        else if (c == 'b') return true;
        else if (c == 'c') return true;
        else if (c == 'd') return true;
        else if (c == 'e') return false;
        else if (c == 'f') return true;
        else if (c == 'g') return true;
        else if (c == 'h') return true;
        else if (c == 'i') return false;
        else if (c == 'j') return true;
        else if (c == 'k') return true;
        else if (c == 'l') return true;
        else if (c == 'm') return true;
        else if (c == 'n') return true;
        else if (c == 'o') return false;
        else if (c == 'p') return true;
        else if (c == 'q') return true;
        else if (c == 'r') return true;
        else if (c == 's') return true;
        else if (c == 't') return true;
        else if (c == 'u') return false;
        else if (c == 'v') return true;
        else if (c == 'w') return true;
        else if (c == 'x') return true;
        else if (c == 'y') return false;
        else if (c == 'z') return true;

        return false;
    }

    public static int GetScrabbleScore(char c) {
        if (c == 'a') return 1;
        else if (c == 'b') return 3;
        else if (c == 'c') return 3;
        else if (c == 'd') return 2;
        else if (c == 'e') return 1;
        else if (c == 'f') return 4;
        else if (c == 'g') return 2;
        else if (c == 'h') return 4;
        else if (c == 'i') return 1;
        else if (c == 'j') return 8;
        else if (c == 'k') return 5;
        else if (c == 'l') return 1;
        else if (c == 'm') return 3;
        else if (c == 'n') return 1;
        else if (c == 'o') return 1;
        else if (c == 'p') return 3;
        else if (c == 'q') return 10;
        else if (c == 'r') return 1;
        else if (c == 's') return 1;
        else if (c == 't') return 1;
        else if (c == 'u') return 1;
        else if (c == 'v') return 4;
        else if (c == 'w') return 4;
        else if (c == 'x') return 8;
        else if (c == 'y') return 4;
        else if (c == 'z') return 10;

        return 0;
    }
}

public struct ScoreData {
    public int lengthScore;
    public int rarityScore;
    public int scrabbleScore;
    public int consonentsScore;
    public int uniqueLettersScore;
    public int keepAllLettersScore;
    public int palindromeScore;
    public int useAllLettersScore;

    public int total => lengthScore + rarityScore + scrabbleScore + consonentsScore + uniqueLettersScore + keepAllLettersScore +
        palindromeScore + useAllLettersScore;
}
