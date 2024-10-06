using UnityEngine;

public class MoveObjects : MonoBehaviour
{
    [ContextMenu("move")]
    public void Move() {
        foreach (Transform child in transform) {
            Vector3 prevPos = child.position;
            prevPos *= 1.333f;
            child.position = prevPos;
        }
    }
}
