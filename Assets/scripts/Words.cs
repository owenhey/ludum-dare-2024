using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "words", menuName = "words", order = 0)]
public class Words : ScriptableObject {
    [SerializeField] private TextAsset file;
    
    private List<string> words;

    public static Words Instance {
        get {
            if (_i == null) {
                _i = Resources.Load("Words") as Words;
                _i.Generate();
            }

            return _i;
        }
    }

    private static Words _i;

    public string GetRandom() {
        return words[Random.Range(0, words.Count)];
    }

    public string GetRandom(int maxPopularity, int minLength, int maxLength) {
        string s;
        do {
            s = words[Random.Range(0, maxPopularity)];
        } while (s.Length > maxLength || s.Length < minLength);

        return s;
    }
    
    public string GetRandom(int maxPopularity, int minLength, int maxLength, char requiredStartLetter) {
        bool allowAppearance = requiredStartLetter is 'z' or 'x';
        maxPopularity *= allowAppearance ? 3 : 1;

        if (requiredStartLetter == 'y') maxPopularity *= 3;
        if (requiredStartLetter == 'q') maxPopularity *= 3;
        if (requiredStartLetter == 'j') maxPopularity *= 3;
        if (requiredStartLetter == 'v') maxPopularity *= 3;
        if (requiredStartLetter == 'k') maxPopularity *= 3;
        
        string s;
        do {
            s = words[Random.Range(0, maxPopularity)];
            if (allowAppearance) {
                if (s.Length <= maxLength && s.Length >= minLength && s.Contains(requiredStartLetter)) break;
            }
        } while (s.Length > maxLength || s.Length < minLength || s[0] != requiredStartLetter);

        return s;
    }
    
    private void Generate() {
        words = new(390025);
        string content = file.text;
        
        string[] lines = file.text.Split('\n');
        foreach (string line in lines) {
            if(string.IsNullOrEmpty(line)) continue;
            bool notvalid = false;
            if (line.Length < 4) {
                continue;
            }
            foreach (char c in line) {
                if ((c < 'a' || c > 'z') && c != 13) {
                    notvalid = true;
                }
            }
            if(notvalid) continue;
            words.Add(line.Trim());
        }
        
        Debug.Log("words length: " + words.Count);
    }

    public int GetRarity(string word) {
        int indexOf = words.IndexOf(word);
        return indexOf;
    }
}
