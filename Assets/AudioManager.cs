using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{ public static AudioManager instance;

    public AudioSource soundEffectAudio;

    public AudioClip jumpSound;
    public AudioClip dashSound;
    public AudioClip hurtSound;
    public AudioClip deathSound;
    public AudioClip winSound;
    public AudioClip enemyHurtSound;
    public AudioClip enemyDeathSound;
    public AudioClip powSound;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(AudioClip sound)
    {
        soundEffectAudio.PlayOneShot(sound);
    }
}