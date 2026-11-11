using UnityEngine;
using System.Collections.Generic;

public class LevelAudioManager : MonoBehaviour
{ public static LevelAudioManager instance;

    public AudioSource levelSoundEffectAudio;

    public AudioClip winSound;

    public AudioClip themeSound;



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            PlaySound(themeSound);

        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(AudioClip sound)
    {
        levelSoundEffectAudio.PlayOneShot(sound);
    }
}
