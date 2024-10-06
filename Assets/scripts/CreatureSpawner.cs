using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CreatureSpawner : MonoBehaviour {
    private List<Creature> allCreatures = new();

    public List<Transform> spawnZones;

    public Creature creatureprefab;
    
    public void Spawn() {
        int numToSpawn = 30;

        var copied = new List<Transform>(spawnZones);

        for (int i = 0; i < numToSpawn; i++) {
            var newCreature = Instantiate(creatureprefab, transform);
            allCreatures.Add(newCreature);
            int randomIndex = Random.Range(0, copied.Count);
            newCreature.Warp(copied[randomIndex].position);
            copied.RemoveAt(randomIndex);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red * .5f;
        foreach (var spawnz in spawnZones) {
            Gizmos.DrawSphere(spawnz.transform.position, 1.5f);
        }
    }

    public void DestroyUncollectedCreatures() {
        for (int i = 0; i < allCreatures.Count; i++) {
            if (allCreatures[i].FollowTarget == null) {
                Destroy(allCreatures[i]);
                allCreatures.RemoveAt(i);
                i--;
            }
        }
    }
}
