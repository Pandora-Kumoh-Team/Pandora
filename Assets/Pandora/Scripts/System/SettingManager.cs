using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace Pandora.Scripts.System
{
    public class SettingManager : MonoBehaviour
    {
        public InputActionAsset inputActionAsset;
        private string _keyBindingPath = "KeyBindings";
        
        public TMP_Dropdown resolutionDropdown;

        private void Awake()
        {
            _keyBindingPath = Application.persistentDataPath + "/KeyBindings.json";
            LoadKeyBindings();
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
    }
}