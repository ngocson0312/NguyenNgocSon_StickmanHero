using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SuperFight
{
    public class AudioManager : Singleton<AudioManager>
    {
        [Header("SoundFX")]
        public AudioClip[] commonSounds;
        [Header("Music")]
        public AudioClip[] musics;
        public AudioSource musicSource;
        public AudioSource soundSource;
        string currentSong;
        

        public void Play(string _name, float volume, bool isloop = false)
        {
            AudioClip s = Array.Find(musics, sound => sound.name == _name);
            if (s != null)
            {
                musicSource.volume = volume;
                musicSource.clip = s;
                musicSource.loop = isloop;
                musicSource.Play();
            }
        }
        public void StopAudio()
        {
            musicSource.Stop();
            soundSource.Stop();
        }
        public void PlayOneShot(string clipName, float volume)
        {
            AudioClip s = Array.Find(commonSounds, sound => sound.name == clipName);
            if (s != null)
            {
                soundSource.PlayOneShot(s, volume);
            }
        }
        public void PlayOneShot(AudioClip clip, float volume)
        {
            if (clip != null)
            {
                soundSource.PlayOneShot(clip, volume);
            }
        }
    }
}

