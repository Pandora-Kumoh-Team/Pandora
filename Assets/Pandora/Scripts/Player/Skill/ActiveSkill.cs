namespace Pandora.Scripts.Player.Skill
{
    public abstract class ActiveSkill : Skill
    {
        public abstract void OnUseSkill();
        public abstract void OnDuringSkill();
        public abstract void OnEndSkill();
    }
}