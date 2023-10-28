using UnityEngine;

namespace Pandora.Scripts.System.Event
{
    /// <summary>
    /// Event가 발생했을 때, 이벤트를 받아서 처리하는 클래스
    /// </summary>
    public interface IEventListener
    {
        void OnEvent(PandoraEventType pandoraEventType, Component sender, object param = null);
    }
}