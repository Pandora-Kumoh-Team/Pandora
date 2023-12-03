using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace Pandora.Scripts.System
{
    public class SoundManager : MonoBehaviour
    {
        // Singleton
        public static SoundManager Instance;
        
        public AudioMixer audioMixer;
        
        [Header("Background Music")]
        // Audio Sources
        public AudioSource audioSource;
        
        // Audio Clips
        public AudioClip mainMusicClip;
        public AudioClip mainMusicLoopClip;
        public AudioClip ingameMusicClip;
        public AudioClip ingameMusicLoopClip;
        public AudioClip ingameAmbienceClip;
        public float ingameAmbienceVolume = 0.5f;

        private List<Coroutine> musicCoroutine;

        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this.gameObject);
                return;
            }
            audioSource = GetComponent<AudioSource>();
            musicCoroutine = new List<Coroutine>();
            // add onSceneLoad method
            SceneManager.sceneLoaded += OnSceneLoaded;
            DontDestroyOnLoad(this.gameObject);
        }
        
        public void OnSceneLoaded(Scene arg0, LoadSceneMode loadSceneMode)
        {
            foreach (var c in musicCoroutine)
            {
                StopCoroutine(c);
            }
            // check if scene is main menu by name
            if (arg0.name == "MainMenu")
            {
                musicCoroutine.Add(StartCoroutine(PlayMainMusic()));
            }
            else
            {
                musicCoroutine.Add(StartCoroutine(PlayIngameMusic()));
                musicCoroutine.Add(StartCoroutine(PlayAmbientSound()));
            }
        }
        
        public IEnumerator PlayMainMusic()
        {
            audioSource.clip = mainMusicClip;
            audioSource.loop = false;
            audioSource.Play();
            yield return new WaitForSeconds(audioSource.clip.length);
            audioSource.clip = mainMusicLoopClip;
            audioSource.loop = true;
            audioSource.Play();
        }
        
        public IEnumerator PlayIngameMusic()
        {
            audioSource.clip = ingameMusicClip;
            audioSource.loop = false;
            audioSource.Play();
            yield return new WaitForSeconds(audioSource.clip.length);
            audioSource.clip = ingameMusicLoopClip;
            audioSource.loop = true;
            audioSource.Play();
        }
        
        public IEnumerator PlayAmbientSound()
        {
            while(true)
            {
                audioSource.PlayOneShot(ingameAmbienceClip, ingameAmbienceVolume);
                yield return new WaitForSeconds(ingameAmbienceClip.length);
            }
        }
        
        public void SetAudioGroupVolume(string parameterName, float volume)
        {
            audioMixer.SetFloat(parameterName, volume);
        }
    }
}