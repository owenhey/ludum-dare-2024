using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
    private static readonly int Worldpos = Shader.PropertyToID("_worldpos");

    private void Awake() {
        Player = this;
        Shader.SetGlobalFloat("_minFog", 20);
        Shader.SetGlobalFloat("_maxFog", 20.5f);
        Shader.SetGlobalVector(Worldpos, Vector3.zero);
    }

    public void Warp(Vector3 pos) {
        CC.enabled = false;
        transform.position = pos;
        CC.enabled = true;
    }

    private bool startedOnCreature = false;

    private void Update() {
        if (Interacting) return;
        
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (Creature.Nearest != null) {
                Creature.Nearest.InteractWith();
            }
        }

        bool mouseDown = Input.GetKeyDown(KeyCode.Mouse0);
        bool isDirectingToMove = Input.GetKey(KeyCode.Mouse0);
        if (isDirectingToMove) {
            Ray ray = Cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100, GroundLayerMask)) {
                if ((hit.collider.gameObject.layer == 8 || EventSystem.current.IsPointerOverGameObject()) && mouseDown) {
                    startedOnCreature = true;
                    return; 
                }

                if (mouseDown) startedOnCreature = false;

                if(!startedOnCreature)
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

    public void ShowOverhead(string text) {
        Debug.Log($"texting! {text}");
        
        IEnumerator delay() {
            RenderWord word = RenderWordsPool.Get();
            word.gameObject.SetActive(true);
            word.transform.position = transform.position + Vector3.up * 1.5f;
            word.ShowWord(text);
            yield return new WaitForSeconds(2.0f);
            word.gameObject.SetActive(false);
        }
        StartCoroutine(delay());
    }

    private void MoveTowards(Vector3 hitpoint) {
        hitpoint = Vector3.ClampMagnitude(hitpoint, 19);
        Vector3 targetPos = Vector3.SmoothDamp(transform.position, hitpoint, ref _vel, Damping, Speed);
        Vector3 torwards = targetPos - transform.position;
        CC.Move(torwards);
    }
}