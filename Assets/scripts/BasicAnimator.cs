using System;
using UnityEngine;

public class BasicAnimator : MonoBehaviour {
    public MeshRenderer MR;
    public Transform Cam;

    public AnimData Idle;
    public AnimData Run;
    
    private Material _material;

    private Vector3 _lastPosition;

    private int _flipped = 2;

    private void Awake() {
        _material = new Material(MR.sharedMaterial);
        MR.material = _material;
        _lastPosition = transform.position;
    }

    private void Update() {
        Vector3 camRight = Cam.right;
        camRight.y = 0;

        Vector3 vel = transform.position - _lastPosition;
        vel.y = 0;
        vel /= Time.deltaTime;

        _lastPosition = transform.position;

        if (vel.magnitude < .01f) {
            AssignAnim(Idle);
            return;
        }
        AssignAnim(Run);
        float dot = Vector3.Dot(vel, camRight);
        Debug.Log(dot);
        SetFlip(dot < 0 ? 1 : 0);
    }

    private void AssignAnim(AnimData data) {
        _material.SetFloat("_animspeed", data.AnimTime);
        _material.SetTexture("_maintex", data.Texture);
    }

    private void SetFlip(int flipped) {
        if (_flipped == flipped) return;
        _material.SetInt("_flipped", flipped);
    }

    private void Reset() {
        Cam = Camera.main.transform;
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(gameObject);
#endif
    }
}

[System.Serializable]
public struct AnimData {
    public Texture2D Texture;
    public float AnimTime;
}
