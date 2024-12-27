
using System;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip sound;
}

public sealed class AudioController : MonoBehaviour
{

    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;
    public static AudioController instance;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);
        if (s == null)
        {
            Debug.Log("Not found Sounds");
        }
        else if (s != null)
        {
            musicSource.clip = s.sound;
            musicSource.Play();
        }
    }
    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }
    private void Start()
    {
        PlayMusic(musicSounds[0].name);
    }
    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);
        if (s == null)
        {
            Debug.Log("Not found Sounds");
        }
        else
        {
            sfxSource.PlayOneShot(s.sound);
        }
    }
    public void changeVolume(float bg, float sfx)
    {
        musicSource.volume = bg / 100;
        sfxSource.volume = sfx / 100;
    }

}
