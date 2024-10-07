using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour {
    public static InteractionManager Instance;
    
    [SerializeField] private RenderWord InteractionRenderWord;

    private List<Interactable> interactables = new();

    private Interactable current;

    private void Start() {
        Instance = this;
    }

    public void RegisterInteractable(Interactable i) {
        if (interactables.Contains(i)) return;
        interactables.Add(i);
    }

    public void UnregisterInteractable(Interactable i) {
        interactables.Remove(i);
        if (current == i) current = null;
    }

    private void Update() {
        if (interactables.Count == 0) {
            InteractionRenderWord.gameObject.SetActive(false);
        }

        if (Movement.Interacting) {
            InteractionRenderWord.gameObject.SetActive(false);
            return;
        }
        
        Interactable closest = null;
        float closestDistance = float.MaxValue;
        for (var index = 0; index < interactables.Count; index++) {
            var interactable = interactables[index];
            if (interactable == null) {
                interactables.RemoveAt(index);
                index--;
                continue;
            }
            float distance = Vector3.SqrMagnitude(Movement.Player.transform.position - interactables[index].GetPosition());
            if (distance < closestDistance) {
                closestDistance = distance;
                closest = interactables[index];
            }
        }
        if (closest == null) return;

        InteractionRenderWord.gameObject.SetActive(true);
        InteractionRenderWord.transform.position = closest.GetWordDisplayPosition();
        if (closest != current) {
            InteractionRenderWord.ShowWord(closest.GetWord());
            current = closest;
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            Movement.Interacting = true;
            current.GetCallback()?.Invoke();
        }
    }
}

public interface Interactable {
    public Vector3 GetPosition();
    
    public Vector3 GetWordDisplayPosition();

    public string GetWord();

    public System.Action GetCallback();
}