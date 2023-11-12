using NotImplementedException = System.NotImplementedException;

namespace Pandora.Scripts.Player.Skill.SkillDetail
{
    /// <summary>
    /// 스킬 미획득 상태
    /// </summary>
    public class SkillNull : ActiveSkill
    {
        public override void OnUseSkill()
        {
            // 스킬 미획득 상태에서 스킬을 사용하면 아무 일도 일어나지 않는다.
        }

        public override void OnDuringSkill()
        {
            // 스킬 미획득 상태에서 스킬을 사용하면 아무 일도 일어나지 않는다.
        }

        public override void OnEndSkill()
        {
            // 스킬 미획득 상태에서 스킬을 사용하면 아무 일도 일어나지 않는다.
        }
    }
}