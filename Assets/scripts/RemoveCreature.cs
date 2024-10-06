using System;
using System.Collections.Generic;
using UnityEngine;

public class RemoveCreature : MonoBehaviour {
    public Camera Cam;
    public LayerMask CreatureLayerMask;

    private void Update() {
        if (Movement.Interacting) return;
        
        bool clicked = Input.GetKeyDown(KeyCode.Mouse0);

        if (clicked) {
            Ray ray = Cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100, CreatureLayerMask)) {
                var creature = hit.collider.GetComponentInParent<Creature>();

                creature.StopFollowing();
            }
        }
    }
}