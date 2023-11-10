using System;
using System.Collections;
using UnityEngine;

namespace Pandora.Scripts.Player.Skill
{
    public abstract class ActiveSkill : Skill
    {
        public void Use()
        {
            OnUseSkill();
        }

        public abstract void OnUseSkill();
        public abstract void OnDuringSkill();
        public abstract void OnEndSkill();
    }
}