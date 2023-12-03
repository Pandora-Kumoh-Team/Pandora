using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Pandora.Scripts.System
{
    public class SettingManager : MonoBehaviour
    {
        public InputActionAsset inputActionAsset;
        private string _keyBindingPath = "KeyBindings";

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
    }
}