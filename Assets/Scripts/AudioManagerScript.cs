using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManagerScript : MonoBehaviour
{
    public AudioSource audioSource;
    public Sprite volumeOn;
    public Sprite volumeOff;
    public Button volumeBtn;

    private void Start() {
        //TODO grab global volume setting (unmute/mute) for volume and set it here
    }

    public void ToggleVolume() {
        if (audioSource.isPlaying) {
            audioSource.Stop();
            volumeBtn.image.sprite = volumeOff;
                
        } else {
            audioSource.Play();
            volumeBtn.image.sprite = volumeOn;
        }
    }
}
