using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Pandora.Scripts.Player.Controller;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

namespace Pandora.Scripts.System
{
    public class SettingManager : MonoBehaviour
    {
        public InputActionAsset inputActionAsset;
        private string _keyBindingPath = "KeyBindings";
        private string _savePath = "Settings";
        
        [Serializable]
        public struct SaveDataStruct
        {
            public float masterVolume;
            public float musicVolume;
            public float sfxVolume;
            public string resolution;
        }
        private SaveDataStruct _saveDataStruct;
        
        public TMP_Dropdown resolutionDropdown;
        public Slider masterVolumeSlider;
        public Slider musicVolumeSlider;
        public Slider sfxVolumeSlider;

        private void Awake()
        {
            _keyBindingPath = Application.persistentDataPath + "/KeyBindings.json";
            _savePath = Application.persistentDataPath + "/Settings.json";
            _saveDataStruct.masterVolume = 120f;
            _saveDataStruct.musicVolume = 120f;
            _saveDataStruct.sfxVolume = 120f;
            LoadSetting();
            StartCoroutine(LateAwake());
        }

        public void SaveSetting()
        {
            // Save Resolution
            _saveDataStruct.resolution = Screen.currentResolution.ToString();
            
            // SaveKeyBindings();
            
            // Save at file
            var str = JsonUtility.ToJson(_saveDataStruct);
            File.WriteAllText(_savePath, str);
        }
        
        public void LoadSetting()
        {
            // Load from file
            if (File.Exists(_savePath))
            {
                var json = File.ReadAllText(_savePath);
                _saveDataStruct = JsonUtility.FromJson<SaveDataStruct>(json);
            }
            else
            {
                SaveSetting();
                return;
            }
            
            // Set Resolution
            var resolution = new Resolution();
            var resolutionString = _saveDataStruct.resolution.Split('@');
            resolution.width = int.Parse(resolutionString[0].Split('x')[0]);
            resolution.height = int.Parse(resolutionString[0].Split('x')[1]);
            Screen.SetResolution(resolution.width, resolution.height, true);
            // Set Resolution Dropdown
            var text = resolution.width + "x" + resolution.height;
            resolutionDropdown.transform.Find("Label").GetComponent<TMP_Text>().text = text;
            
            // Set Volume
            masterVolumeSlider.value = _saveDataStruct.masterVolume;
            musicVolumeSlider.value = _saveDataStruct.musicVolume;
            sfxVolumeSlider.value = _saveDataStruct.sfxVolume;
            OnMasterVolumeChanged(_saveDataStruct.masterVolume);
            OnMusicVolumeChanged(_saveDataStruct.musicVolume);
            OnSfxVolumeChanged(_saveDataStruct.sfxVolume);
            
            // LoadKeyBindings();
        }
        
        private IEnumerator LateAwake()
        {
            yield return null;
            masterVolumeSlider.value = _saveDataStruct.masterVolume;
            musicVolumeSlider.value = _saveDataStruct.musicVolume;
            sfxVolumeSlider.value = _saveDataStruct.sfxVolume;
            OnMasterVolumeChanged(_saveDataStruct.masterVolume);
            OnMusicVolumeChanged(_saveDataStruct.musicVolume);
            OnSfxVolumeChanged(_saveDataStruct.sfxVolume);
            // PlayerManager.Instance.SetPlayerInput(inputActionAsset);
        }
        
        public void SaveKeyBindings()
        {
            var json = inputActionAsset.SaveBindingOverridesAsJson();
            File.WriteAllText(_keyBindingPath, json);
        }
        
        public void LoadKeyBindings()
        {
            if (File.Exists(_keyBindingPath))
            {
                var json = File.ReadAllText(_keyBindingPath);
                inputActionAsset.LoadBindingOverridesFromJson(json);
            }
        }
        
        public void ChangeResolution(Int32 index)
        {
            var xy= resolutionDropdown.options[index].text.Split('x');
            var x = int.Parse(xy[0]);
            var y = int.Parse(xy[1]);
            Screen.SetResolution(x, y, Screen.fullScreen);
        }
        
        public void OnMasterVolumeChanged(float changedValue)
        {
            _saveDataStruct.masterVolume = changedValue;
            changedValue /= 100;
            var dbToRate = changedValue != 0 ? Mathf.Log10(changedValue) * 20 : -80;
            SoundManager.Instance.SetAudioGroupVolume("MasterVolume", dbToRate);
        }
        
        public void OnMusicVolumeChanged(float changedValue)
        {
            _saveDataStruct.musicVolume = changedValue;
            changedValue /= 100;
            var dbToRate = changedValue != 0 ? Mathf.Log10(changedValue) * 20 : -80;
            SoundManager.Instance.SetAudioGroupVolume("MusicVolume", dbToRate);
        }
        
        public void OnSfxVolumeChanged(float changedValue)
        {
            _saveDataStruct.sfxVolume = changedValue;
            changedValue /= 100;
            var dbToRate = changedValue != 0 ? Mathf.Log10(changedValue) * 20 : -80;
            SoundManager.Instance.SetAudioGroupVolume("SfxVolume", dbToRate);
        }
    }
}