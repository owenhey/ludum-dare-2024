using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class Creature : MonoBehaviour {
    public RenderToken token;
    public CreatureMinigame Minigame;

    public static Creature Nearest;

    private string selectedWord;

    public char Char;

    public bool lookingForPlayer = true;

    private RenderWord renderWord;

    public CharacterController CC;

    public float speed = 3;

    private Vector3 startLocation;

    private static Collider[] results = new Collider[8];
    public LayerMask TreesAndRocks;

    private bool hasTarget;
    private Vector3 target;
    
    private Vector3 _vel;

    public Transform FollowTarget;
    public float FollowDistance;
    
    
    private void Start() {
        Char = (char)('a' + Random.Range(0, 25));
        token.ShowLetter(Char);

        Minigame.enabled = false;
        startLocation = transform.position;
        StartCoroutine(WanderCoroutine());
        FollowDistance *= Random.Range(.65f, 1.3f);
    }

    private IEnumerator WanderCoroutine() {
        while (true) {
            float wait = Random.Range(3, 8);
            yield return new WaitForSeconds(wait);
            
            // Choose a random spot nearby
            while (true) {
                // Make sure no trees are nearby
                var randomCircle = Random.insideUnitCircle;
                target = startLocation + new Vector3(randomCircle.x, 0, randomCircle.y) * 2;
                int numHit = Physics.OverlapSphereNonAlloc(target, 1, results, TreesAndRocks);
                if(Vector3.SqrMagnitude(transform.position - target) < 2.0f) continue;
                if (numHit == 0) break;
            }

            hasTarget = true;
        }
    }

    private void Update() {
        if (FollowTarget) {
            GoTowardsTarg();
            return;
        }
        
        if (!hasTarget) return;
        if (Nearest == this) return;
        if (Minigame.enabled) return;
        
        Vector3 targetPos = Vector3.SmoothDamp(transform.position, target, ref _vel, .15f, speed);
        Vector3 torwards = targetPos - transform.position;
        CC.Move(torwards);

        if (Vector3.SqrMagnitude(transform.position - target) < .1f) {
            hasTarget = false;
        }
    }

    private void GoTowardsTarg() {
        Vector3 towardsMe = ((transform.position - FollowTarget.position).normalized) * FollowDistance;
        
        Vector3 targetPos = Vector3.SmoothDamp(transform.position, FollowTarget.position + towardsMe, ref _vel, .15f, speed);

        Vector3 torwards = targetPos - transform.position;
        if (Vector3.SqrMagnitude(FollowTarget.position - transform.position) < .1f) {
            return;
        }
        CC.Move(torwards);
    }

    public void InteractWith() {
        if (Minigame.enabled) return;
        
        
        Minigame.enabled = true;
        lookingForPlayer = false;
        Minigame.StartMinigame(renderWord);
    }

    public void ForceNearest() {
        Nearest = this;
        renderWord = RenderWordsPool.Get();
        renderWord.gameObject.SetActive(true);
        renderWord.SetFollow(transform, Vector3.up);
        renderWord.ShowWord("!");
    }

    public void ForceHide() {
        renderWord.gameObject.SetActive(false);
    }

    public void LitteHop() {
        
        transform.DOMoveY(transform.position.y + .5f, .15f).SetEase(Ease.OutQuad).OnComplete(() => {
            transform.DOMoveY(0, .15f).SetEase(Ease.InQuad);
        });
    }
    
    public void FollowPlayer() {
        if (Movement.Player.CreaturesFollowing.Count == 0) {
            FollowTarget = Movement.Player.transform;
        }
        else {
            FollowTarget = Movement.Player.CreaturesFollowing[^1].transform;
        }
        Movement.Player.CreaturesFollowing.Add(this);
    }

    public void OnTriggerEnter(Collider other) {
        if(!lookingForPlayer) return;
        
        if (other.CompareTag("Player")) {
            if (Nearest != this && Nearest != null) {
                Nearest.ForceHide();
            }
            Nearest = this;
            renderWord = RenderWordsPool.Get();
            renderWord.gameObject.SetActive(true);
            renderWord.SetFollow(transform, Vector3.up);
            renderWord.ShowWord("!");
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = new Color(.5f, .5f, .5f, .5f);
        Gizmos.DrawSphere(transform.position, 2.0f);
    }

    public void OnTriggerExit(Collider other) {
        if(!lookingForPlayer) return;
        
        if (other.CompareTag("Player")) {
            renderWord.gameObject.SetActive(false);
            if (Nearest == this) {
                Nearest = null;
            }

        }
    }
}
