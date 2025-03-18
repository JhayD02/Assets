using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class AudioManager : NetworkBehaviour
{
    [SerializeField] GameObject lobbyMusic;
    [SerializeField] GameObject inGameMusic;

    [SerializeField] AudioSource soundEffects;
    public AudioClip punchSound;
    public AudioClip shootSound;
    public AudioClip jumpSound;
    public AudioClip deathSound;

    public AudioClip damageSound;

    [SerializeField] AudioSource walkSoundEffects;

    void Start()
    {
        // No need to add listener here, as volume is controlled by VolumeChanger
    }

    public static void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    public override void OnStartClient()
    {
        lobbyMusic.SetActive(false);
        inGameMusic.SetActive(true);
        base.OnStartClient();

        if (!isLocalPlayer) return; // Ensure only the local player controls music
    }

    public override void OnStopClient()
    {
        inGameMusic.SetActive(false);
        lobbyMusic.SetActive(true);

        base.OnStopClient();

        if (!isLocalPlayer) return; // Ensure only the local player controls music
    }

    public void PlaySFX(AudioClip clip)
    {
        soundEffects.PlayOneShot(clip);
    }

    public void PlayWalkSound()
    {
        walkSoundEffects.Play();
    }

    public void StopWalkSound()
    {
        walkSoundEffects.Stop();
    }
}