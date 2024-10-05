using UnityEngine;

public class RenderToken : MonoBehaviour {
    private bool _copiedMat;
    
    public MeshRenderer TokenMR;
    private Material _tokenMat;
    
    public void ShowLetter(int index) {
        if (!_copiedMat) {
            _tokenMat = new Material(TokenMR.sharedMaterial);
            TokenMR.material = _tokenMat;
        }
        _tokenMat.SetFloat("_letter", index + .1f);
    }
}
