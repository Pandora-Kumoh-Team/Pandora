using UnityEngine;

namespace Pandora.Scripts.Player.Skill
{
    public abstract class PassiveSkill : Skill
    {
        public abstract void OnGetSkill();

        public abstract void OnLoseSkill();

    }
}