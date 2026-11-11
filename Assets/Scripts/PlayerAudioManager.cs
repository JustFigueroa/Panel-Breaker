using UnityEngine;
using System.Collections.Generic;

public class PlayerAudioManager : MonoBehaviour
{ public static PlayerAudioManager instance;

    public AudioSource playerSoundEffectAudio;
    public AudioClip runSound;

    public AudioClip jumpGruntSound;
    public AudioClip jumpSound;

    public AudioClip dashGruntSound;
    public AudioClip dashSound;

    public AudioClip hurtSound;
    public AudioClip deathSound;
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
        playerSoundEffectAudio.PlayOneShot(sound);
    }
}
