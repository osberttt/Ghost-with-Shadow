using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    [Header("Audio")]
    public float volumeSFX = 1; // Between 0->1
    public float volumeMusic = 1;
    public GameObject audioContainer;
    public GameObject audioSource;

    public AudioSource musicSource;
    
    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Function that plays the SFX
    public void PlaySFX(AudioClip audioClip, bool randomizePitch=false, float pitch1=0.95f, float pitch2=1.05f ) {
        // Create audio source object as child of audioContainer
        GameObject localAudioSource = Instantiate(audioSource);
        localAudioSource.transform.SetParent(audioContainer.transform); // Set audioContainer as parent
        AudioSource localAudioSourceComponent = localAudioSource.GetComponent<AudioSource>(); // Get component

        // Change pitch if pitchRange true
        if (randomizePitch) localAudioSourceComponent.pitch = Random.Range(pitch1, pitch2);

        // Play sound
        localAudioSourceComponent.volume = volumeSFX;
        localAudioSourceComponent.clip = audioClip;
        localAudioSourceComponent.Play();

        // Destroy object
        Destroy(localAudioSource, localAudioSourceComponent.clip.length);
    }

    public void PlayMusic(AudioClip musicClip)
    {
        musicSource.clip = musicClip;
        musicSource.loop = true;
        musicSource.volume = volumeMusic;
        musicSource.Play();
    }
}
