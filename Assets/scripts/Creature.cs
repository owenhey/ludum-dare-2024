using UnityEngine;

public class Creature : MonoBehaviour {
    public RenderToken token;

    private void Start() {
        token.ShowLetter((char)('a' + Random.Range(0, 30)));
    }
}
