using UnityEngine;

namespace DefaultNamespace {
    public class SpriteRendererSwitch : MonoBehaviour {
        public float AnimSpeed = .45f;
        public Sprite Sprite1;
        public Sprite Sprite2;

        public SpriteRenderer SR;
        
        private void Update() {
            float t = Time.time / AnimSpeed;
            SR.sprite = ((int)t) % 2 == 0 ? Sprite1 : Sprite2;
        }
    }
}