using System;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
    public float Speed = 3;
    public float Damping = .1f;
    public CharacterController CC;
    
    public Camera Cam;
    public LayerMask GroundLayerMask;

    
    private Vector3 _vel;

    public static bool Interacting = false;

    public List<Creature> CreaturesFollowing = new();

    public static Movement Player;

    private void Awake() {
        Player = this;
    }

    public void Warp(Vector3 pos) {
        CC.enabled = false;
        transform.position = pos;
        CC.enabled = true;
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Q)) Fader.instance.FadeWithFunction(null);
        
        if (Interacting) return;
        
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (Creature.Nearest != null) {
                Creature.Nearest.InteractWith();
            }
        }
        
        bool isDirectingToMove = Input.GetKey(KeyCode.Mouse0);
        if (isDirectingToMove) {
            Ray ray = Cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100, GroundLayerMask)) {
                MoveTowards(hit.point);
            }
        }
        else {
            Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

            Vector3 camForwardNoZ = Cam.transform.forward;
            camForwardNoZ.y = 0;
            
            Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, camForwardNoZ);
            Vector3 move = rotation * input;
            
            if (input != Vector3.zero) {
                MoveTowards(transform.position + move);
            }
        }
    }

    private void MoveTowards(Vector3 hitpoint) {
        Vector3 targetPos = Vector3.SmoothDamp(transform.position, hitpoint, ref _vel, Damping, Speed);
        Vector3 torwards = targetPos - transform.position;
        CC.Move(torwards);
    }
}