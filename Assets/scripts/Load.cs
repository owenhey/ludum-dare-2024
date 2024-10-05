using System;
using UnityEngine;

namespace DefaultNamespace {
    public class Load : MonoBehaviour {
        private void Awake() {
            Words.Instance.GetRandom();
        }
    }
}