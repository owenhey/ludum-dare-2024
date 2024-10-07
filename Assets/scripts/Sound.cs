using System;
using TMPro;
using UnityEngine;

public class Sound : MonoBehaviour {
    public static Sound I;

    public AudioSource Music;
    public TextMeshProUGUI MusicText;

    public Sounder Popsound;
    public Sounder Yessound;
    public Sounder Nosound;
    public Sounder Happysound;
    public Sounder Sadsound;
    public Sounder Swishsound;
    public Sounder Knocksound;
    public Sounder Knocksound2;
    public Sounder Grasssound;
    public Sounder Footstepsound;

    public AudioSource CountUp;

    public void Awake() {
        I = this;
    }

    public void ToggleSound() {
        PlayKnock2();
        if (Music.isPlaying) {
            Music.Pause();
            MusicText.text = "toggle music on";
        }
        else {
            Music.Play();
            MusicText.text = "toggle music off";
        }
    }

    public void StartCountUp() {
        CountUp.time = 0;
        CountUp.Play();
    }

    public void StopCountUp() {
        CountUp.Stop();
    }

    public void PlayPop(float delay = 0) {
        Popsound.Play(delay);
    }
    
    public void PlayYes(float delay = 0){
        Yessound.Play(delay);
    }
    public void PlayNo(float delay = 0){
        Nosound.Play(delay);
    }

    public void PlayHapppy(float delay = 0) {
        Happysound.Play(delay);
    }
    public void PlaySad(float delay = 0){
        Sadsound.Play(delay);     
    }
    
    public void PlaySwish(float delay = 0){
        Swishsound.Play(delay);     
    }
    
    public void PlayKnock(float delay = 0){
        Knocksound.Play(delay);     
    }
    
    public void PlayKnock2(float delay = 0){
        Knocksound2.Play(delay);     
    }

    public void PlayGrass(float delay = 0) {
        Grasssound.Play(delay);
    }
    
    public void PlayFootstep(float delay = 0) {
        Footstepsound.Play(delay);
    }
}
