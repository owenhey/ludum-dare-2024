using System;
using System.Collections.Generic;
using UnityEngine;

public class PanningCams : MonoBehaviour
{
    [System.Serializable]
    public struct Pan {
        public Transform cam;
        public Transform pos1;
        public Transform pos2;
    }

    public List<Pan> panners;
    private int panindex;

    private void Start() {
        for (int i = 0; i < panners.Count; i++) {
            bool active = i == 0;
            panners[i].cam.parent.gameObject.SetActive(active);
        }
    }

    public void Update() {
        var pan = panners[panindex % panners.Count];
        Vector3 direction = (pan.pos2.position - pan.pos1.position).normalized;
        pan.cam.position += direction * (Time.deltaTime * .25f);

        if ((pan.cam.position - pan.pos2.position).sqrMagnitude < .25f) {
            panindex++;
            panners[panindex % panners.Count].cam.position = panners[panindex % panners.Count].pos1.position;
            panners[(panindex - 1) % panners.Count].cam.parent.gameObject.SetActive(false);
            panners[panindex % panners.Count].cam.parent.gameObject.SetActive(true);
        }
    }
}
