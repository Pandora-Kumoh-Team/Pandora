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
        skillList[0] = new Skill(000, skillIcons[0], "���ݷ� ��ȭ", "normal", false, "���ݷ��� 10% �����Ѵ�.");
        skillList[1] = new Skill(001, skillIcons[1], "ü�� ��ȭ", "normal", false, "ü���� 10% �����Ѵ�.");
        skillList[2] = new Skill(002, skillIcons[2], "�̵��ӵ� ��ȭ", "normal", false, "�̵��ӵ��� 5% �����Ѵ�.");
        skillList[3] = new Skill(003, skillIcons[3], "���� ��ȭ", "normal", false, "������ 5% �����Ѵ�.");
        skillList[4] = new Skill(004, skillIcons[4], "���ݼӵ� ��ȭ", "normal", false, "���ݼӵ��� 15% �����Ѵ�.");
        skillList[5] = new Skill(005, skillIcons[5], "ġ��Ÿ Ȯ�� ��ȭ", "normal", false, "ġ��Ÿ Ȯ���� 10% �����Ѵ�.");
        skillList[6] = new Skill(006, skillIcons[6], "ȸ���� ��ȭ", "normal", false, "ȸ������ 20% �����Ѵ�.");
        skillList[7] = new Skill(007, skillIcons[7], "ȿ���� ����", "normal", false, "�±� �� HP ȸ������ 20% �����Ѵ�.");
        skillList[8] = new Skill(008, skillIcons[8], "����", "normal", false, "���Ÿ� ������ ����ü ������ 1 �����Ѵ�.");
        skillList[9] = new Skill(009, skillIcons[9], "����", "normal", false, "���Ÿ� ������ ���� �����Ѵ�.");
        skillList[10] = new Skill(010, skillIcons[10], "����", "normal", false, "�ٰŸ� �������� ������ ���ظ� ���� �� �ִ� 2���� �ֺ� ���� �߰��� Ÿ���ϴ� ������ ����ŵ�ϴ�.");
        skillList[11] = new Skill(011, skillIcons[11], "�ܻ�", "normal", false, "�ٰŸ� ������ ���ݷ��� 20%��ŭ �߰� Ÿ���� ������.");

        skillList[12] = new Skill(012, skillIcons[12], "���ݷ� ��ȭ", "rare", false, "���ݷ��� 20% �����Ѵ�.");
        skillList[13] = new Skill(013, skillIcons[13], "ü�� ��ȭ", "rare", false, "ü���� 20% �����Ѵ�.");
        skillList[14] = new Skill(014, skillIcons[14], "�̵��ӵ� ��ȭ", "rare", false, "�̵��ӵ��� 10% �����Ѵ�.");
        skillList[15] = new Skill(015, skillIcons[15], "���� ��ȭ", "rare", false, "������ 10% �����Ѵ�.");
        skillList[16] = new Skill(016, skillIcons[16], "���ݼӵ� ��ȭ", "rare", false, "���ݼӵ��� 30% �����Ѵ�.");
        skillList[17] = new Skill(017, skillIcons[17], "ġ��Ÿ Ȯ�� ��ȭ", "rare", false, "ġ��Ÿ Ȯ���� 30% �����Ѵ�.");

        skillList[18] = new Skill(018, skillIcons[18], "���ݷ� ��ȭ", "unique", false, "���ݷ��� 40% �����Ѵ�.");
        skillList[19] = new Skill(019, skillIcons[19], "ü�� ��ȭ", "unique", false, "ü���� 40% �����Ѵ�.");
        skillList[20] = new Skill(020, skillIcons[20], "�̵��ӵ� ��ȭ", "unique", false, "�̵��ӵ��� 20% �����Ѵ�.");
        skillList[21] = new Skill(021, skillIcons[21], "���� ��ȭ", "unique", false, "������ 20% �����Ѵ�.");
        skillList[22] = new Skill(022, skillIcons[22], "���ݼӵ� ��ȭ", "unique", false, "���ݼӵ��� 60% �����Ѵ�.");
        skillList[23] = new Skill(023, skillIcons[23], "ġ��Ÿ Ȯ�� ��ȭ", "unique", false, "ġ��Ÿ Ȯ���� 60% �����Ѵ�.");

        skillList[24] = new Skill(024, skillIcons[24], "���ݷ� ��ȭ", "legendary", false, "���ݷ��� 100% �����Ѵ�.");
        skillList[25] = new Skill(025, skillIcons[25], "ü�� ��ȭ", "legendary", false, "ü���� 100% �����Ѵ�.");
        skillList[26] = new Skill(026, skillIcons[26], "�̵��ӵ� ��ȭ", "legendary", false, "�̵��ӵ��� 40% �����Ѵ�.");
        skillList[27] = new Skill(027, skillIcons[27], "���� ��ȭ", "legendary", false, "������ 40% �����Ѵ�.");
        skillList[28] = new Skill(028, skillIcons[28], "���ݼӵ� ��ȭ", "legendary", false, "���ݼӵ��� 100% �����Ѵ�.");
        skillList[29] = new Skill(029, skillIcons[29], "ġ��Ÿ Ȯ�� ��ȭ", "legendary", false, "ġ��Ÿ Ȯ���� 100% �����Ѵ�.");
        #endregion

        #region active

        #endregion

    }

    public bool GetisActive(int index)
    {
        return skillList[index]._isActive;
    }
}
