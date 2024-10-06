using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace DefaultNamespace {
    public class IncrementingScore : MonoBehaviour {
        public TextMeshProUGUI field;

        private int current = 0;

        private void OnEnable() {
            field.text = "";
        }

        public void CountUpTo(int count) {
            current = 0;
            DOTween.To(() => current, (x) => current = x, count, 2.0f).SetEase(Ease.OutCubic).OnUpdate(UpdateText);
        }

        private void UpdateText() {
            field.text = current.ToString();
        }
    }
}