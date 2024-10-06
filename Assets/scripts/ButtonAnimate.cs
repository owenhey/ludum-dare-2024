using UnityEngine;
using UnityEngine.UI;

public class ButtonAnimate : MonoBehaviour {
    public Image image;
    public Sprite sprite1;
    public Sprite sprite2;
    public float animTime = .65f;

    public void Update() {
        float time = Time.time * animTime;
        image.sprite = (int)(time) % 2 == 0 ? sprite1 : sprite2;
    }
}
