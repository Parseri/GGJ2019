using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioClips : MonoBehaviour {

    public AudioClip simpact;
    public AudioClip sMove2;
    public AudioClip sMove;
    public AudioClip sDash;
    public AudioClip sEnd;
    AudioSource audioSource;

    void Start() {
        audioSource = GetComponent<AudioSource>();
    }


    public void onPlayerEnd() {
        onPlayAudio(sEnd);
    }

    public void onPlayerimpact() {
        onPlayAudio(simpact);
    }

    public void onPlayerMove() {
        onPlayAudio(sMove);
    }

    public void onPlayerMove2() {
        onPlayAudio(sMove2);
    }

    public void onPlayerDash() {
        onPlayAudio(sDash);
    }

    void onPlayAudio(AudioClip file) {
        audioSource.PlayOneShot(file, 0.7F);
    }
}
