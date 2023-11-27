using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace Pandora.Scripts.System
{
    public class SoundManager : MonoBehaviour
    {
        // Singleton
        public static SoundManager Instance;
        
        // Audio Sources
        public AudioSource mainMusic;
        public AudioSource mainMusicLoop;
        public AudioSource ingameMusic;
        public AudioSource ingameMusicLoop;
        
        // Audio Clips
        public AudioClip mainMusicClip;
        public AudioClip mainMusicLoopClip;
        public AudioClip ingameMusicClip;
        public AudioClip ingameMusicLoopClip;

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
            
            DontDestroyOnLoad(this.gameObject);
        }
        public void Start()
        {
            mainMusic.clip = mainMusicClip;
            mainMusicLoop.clip = mainMusicLoopClip;
            ingameMusic.clip = ingameMusicClip;
            ingameMusicLoop.clip = ingameMusicLoopClip;
            PlayMainMusic();
            
            // add onSceneLoad method
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        
        public void OnSceneLoaded(Scene arg0, LoadSceneMode loadSceneMode)
        {
            // check if scene is main menu by name
            if (arg0.name == "MainMenu")
            {
                PlayMainMusic();
            }
            else
            {
                PlayIngameMusic();
            }
        }
        
        public void PlayMainMusic()
        {
            mainMusic.Play();
            mainMusicLoop.PlayDelayed(mainMusic.clip.length);
        }
        
        public void PlayIngameMusic()
        {
            ingameMusic.Play();
            ingameMusicLoop.PlayDelayed(ingameMusic.clip.length);
        }
    }
}