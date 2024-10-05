using UnityEngine;

public class LookTowardsCam : MonoBehaviour {
    public Transform Cam;
    public bool LookUp;
    
    private void LateUpdate() {
        Vector3 towardsCamera = Cam.position - transform.position;

        if(!LookUp)
            towardsCamera.y = 0;
        transform.LookAt(transform.position + towardsCamera);
    }

    private void Reset() {
        Cam = Camera.main.transform;
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(gameObject);
#endif
    }
}