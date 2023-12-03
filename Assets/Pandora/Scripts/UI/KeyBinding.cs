using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Pandora.Scripts.UI
{
    public class KeyBinding : MonoBehaviour
    {
        [SerializeField] private InputActionReference rebindAction;
        [SerializeField] private bool isComposite;
        [SerializeField] private int compositeIndex;
        private TextMeshProUGUI _text;

        private InputActionRebindingExtensions.RebindingOperation _rebindingOperation;

        private void Awake()
        {
            _text = transform.Find("Key").GetComponent<TextMeshProUGUI>();
            StartCoroutine(LateAwake());
        }

        private IEnumerator LateAwake()
        {
            yield return null;
            if(!isComposite)
                _text.text = "[" + rebindAction.action.GetBindingDisplayString() + "]";
            else
                _text.text = "[" + rebindAction.action.GetBindingDisplayString(compositeIndex) + "]";
        }

        public void Rebind()
        {
            _text.text = "[...]";
            
            if (!isComposite)
            {
                _rebindingOperation = rebindAction.action.PerformInteractiveRebinding()
                    .WithControlsExcluding("Mouse")
                    .OnMatchWaitForAnother(0.1f)
                    .OnComplete(_ => RebindComplete())
                    .Start();
            }
            else
            {
                _rebindingOperation = rebindAction.action.PerformInteractiveRebinding()
                    .WithTargetBinding(compositeIndex)
                    .OnMatchWaitForAnother(.1f)
                    .OnComplete(_ => RebindComplete())
                    .Start();
            }
        }
        
        private void RebindComplete()
        {
            var bindingIndex = 0;
       
            if (!isComposite)
                bindingIndex = rebindAction.action.GetBindingIndexForControl(rebindAction.action.controls[0]);
            else
                bindingIndex = compositeIndex;
       
            _text.text = "[" + InputControlPath.ToHumanReadableString(
                rebindAction.action.bindings[bindingIndex].effectivePath,
                InputControlPath.HumanReadableStringOptions.OmitDevice) + "]";
            _rebindingOperation.Dispose();
        }

        private void InputBindingText(InputAction inputAction)
        {
        }
    }
}