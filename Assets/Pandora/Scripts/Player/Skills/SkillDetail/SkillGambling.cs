using Pandora.Scripts.Enemy;
using Pandora.Scripts.Player.Controller;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Pandora.Scripts.Player.Skill.SkillDetail
{
    public class SkillGambling : ActiveSkill
    {
        private PlayerController _playerController;
        GameObject effect1;
        GameObject effect2;
        GameObject effect3;

        [Header("피해량")]
        public float damageR;

        [Header("얻는 공격력")]
        public float damage;

        public override void OnUseSkill()
        {
            transform.localPosition = Vector3.zero;
            effect1 = transform.Find("Effect1").gameObject;
            effect2 = transform.Find("Effect2").gameObject;
            effect3 = transform.Find("Effect3").gameObject;
            effect1.transform.localPosition = new Vector3(0, 1.2f, 0);
            effect2.transform.localPosition = new Vector3(0.15f, 0.3f, 0);
            effect3.transform.localPosition = new Vector3(0, 0, 0);

            _playerController = ownerPlayer.GetComponent<PlayerController>();

            StartCoroutine(GamblingCoroutine());
        }

        public override void OnDuringSkill()
        {
            // Do nothing
        }

        public override void OnEndSkill()
        {
            // Do nothing
        }

        private IEnumerator GamblingCoroutine()
        {
            effect1.SetActive(true);
            yield return new WaitForSeconds(1.0f);
            effect1.SetActive(false);

            int random = Random.Range(0, 2);

            if (random < 1) // random == 0 , 실패 ( hp 감소 )
            {
                _playerController.playerCurrentStat.NowHealth -= damageR;
                _playerController.CallHealthChangedEvent();
                effect2.SetActive(true);
                yield return new WaitForSeconds(1.0f);
                effect2.SetActive(false);
            }
            else // random == 1 , 성공( 공격력 증가 )
            {
                _playerController.playerCurrentStat.BaseDamage += damage;
                _playerController.CallHealthChangedEvent();
                effect3.SetActive(true);
                yield return new WaitForSeconds(1.0f);
                effect3.SetActive(false);
            }
        }
    }
}

