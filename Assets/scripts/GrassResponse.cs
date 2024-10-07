using DG.Tweening;
using UnityEngine;

public class GrassResponse : MonoBehaviour {
    public Transform Model;
    public void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            Sound.I.PlayGrass();
            Model.DOShakeRotation(.35f, Vector3.one * 20, 10);
        }
    }
}
