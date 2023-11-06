using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillList
{
    private readonly int skillNum = 100;
    public Skill[] skillList;
    public Sprite[] skillIcons;

    public SkillList()
    {
        skillList = new Skill[skillNum];
        skillIcons = new Sprite[skillNum];
        Generate();
    }

    private void Generate()
    {
        // [Skill Code]
        // 000 ~ : passive
        // 100 ~ : active
        // id, icon, name, level, grade, isActive, func

        #region passive
        skillList[0] = new Skill(000, skillIcons[0], "공격력 강화", "normal", false, "공격력이 10% 증가한다.");
        skillList[1] = new Skill(001, skillIcons[1], "체력 강화", "normal", false, "체력이 10% 증가한다.");
        skillList[2] = new Skill(002, skillIcons[2], "이동속도 강화", "normal", false, "이동속도가 5% 증가한다.");
        skillList[3] = new Skill(003, skillIcons[3], "방어력 강화", "normal", false, "방어력이 5% 증가한다.");
        skillList[4] = new Skill(004, skillIcons[4], "공격속도 강화", "normal", false, "공격속도가 15% 증가한다.");
        skillList[5] = new Skill(005, skillIcons[5], "치명타 확률 강화", "normal", false, "치명타 확률이 10% 증가한다.");
        skillList[6] = new Skill(006, skillIcons[6], "회피율 강화", "normal", false, "회피율이 20% 증가한다.");
        skillList[7] = new Skill(007, skillIcons[7], "효율적 전투", "normal", false, "태그 시 HP 회복량이 20% 증가한다.");
        skillList[8] = new Skill(008, skillIcons[8], "복제", "normal", false, "원거리 공격의 투사체 개수가 1 증가한다.");
        skillList[9] = new Skill(009, skillIcons[9], "관통", "normal", false, "원거리 공격이 적을 관통한다.");
        skillList[10] = new Skill(010, skillIcons[10], "뇌전", "normal", false, "근거리 공격으로 적에게 피해를 입힐 시 최대 2명의 주변 적을 추가로 타격하는 번개를 일으킵니다.");
        skillList[11] = new Skill(011, skillIcons[11], "잔상", "normal", false, "근거리 공격이 공격력의 20%만큼 추가 타격을 입힌다.");

        skillList[12] = new Skill(012, skillIcons[12], "공격력 강화", "rare", false, "공격력이 20% 증가한다.");
        skillList[13] = new Skill(013, skillIcons[13], "체력 강화", "rare", false, "체력이 20% 증가한다.");
        skillList[14] = new Skill(014, skillIcons[14], "이동속도 강화", "rare", false, "이동속도가 10% 증가한다.");
        skillList[15] = new Skill(015, skillIcons[15], "방어력 강화", "rare", false, "방어력이 10% 증가한다.");
        skillList[16] = new Skill(016, skillIcons[16], "공격속도 강화", "rare", false, "공격속도가 30% 증가한다.");
        skillList[17] = new Skill(017, skillIcons[17], "치명타 확률 강화", "rare", false, "치명타 확률이 30% 증가한다.");

        skillList[18] = new Skill(018, skillIcons[18], "공격력 강화", "unique", false, "공격력이 40% 증가한다.");
        skillList[19] = new Skill(019, skillIcons[19], "체력 강화", "unique", false, "체력이 40% 증가한다.");
        skillList[20] = new Skill(020, skillIcons[20], "이동속도 강화", "unique", false, "이동속도가 20% 증가한다.");
        skillList[21] = new Skill(021, skillIcons[21], "방어력 강화", "unique", false, "방어력이 20% 증가한다.");
        skillList[22] = new Skill(022, skillIcons[22], "공격속도 강화", "unique", false, "공격속도가 60% 증가한다.");
        skillList[23] = new Skill(023, skillIcons[23], "치명타 확률 강화", "unique", false, "치명타 확률이 60% 증가한다.");

        skillList[24] = new Skill(024, skillIcons[24], "공격력 강화", "legendary", false, "공격력이 100% 증가한다.");
        skillList[25] = new Skill(025, skillIcons[25], "체력 강화", "legendary", false, "체력이 100% 증가한다.");
        skillList[26] = new Skill(026, skillIcons[26], "이동속도 강화", "legendary", false, "이동속도가 40% 증가한다.");
        skillList[27] = new Skill(027, skillIcons[27], "방어력 강화", "legendary", false, "방어력이 40% 증가한다.");
        skillList[28] = new Skill(028, skillIcons[28], "공격속도 강화", "legendary", false, "공격속도가 100% 증가한다.");
        skillList[29] = new Skill(029, skillIcons[29], "치명타 확률 강화", "legendary", false, "치명타 확률이 100% 증가한다.");
        #endregion

        #region active

        #endregion

    }

    public bool GetisActive(int index)
    {
        return skillList[index]._isActive;
    }
}
