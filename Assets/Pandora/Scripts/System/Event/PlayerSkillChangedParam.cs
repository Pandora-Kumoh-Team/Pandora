using Pandora.Scripts.Player.Skill;

namespace Pandora.Scripts.System.Event
{
    public struct PlayerSkillChangedParam
    {
        public Skill Skill;
        public int playerNumber;
        public int skillIndex;

        public PlayerSkillChangedParam(Skill skill, int playerNumber, int skillIndex)
        {
            Skill = skill;
            this.playerNumber = playerNumber;
            this.skillIndex = skillIndex;
        }
    }
}