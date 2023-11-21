using System;
using Pandora.Scripts.Player;
using Pandora.Scripts.Player.Controller;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pandora.Scripts.UI
{
    public class PermanentStatContainerUi : MonoBehaviour
    {
        public int playerNum;
        
        private Slider maxHP;
        private Slider baseDamage;
        private Slider speed;
        private Slider attackSpeed;
        private TextMeshProUGUI leftPoints;
        private TextMeshProUGUI lv;
        private PlayerStat playerStat;
        private Animator playerPreviewAnimator;
        private static readonly int IsMelee = Animator.StringToHash("IsMelee");

        private void Awake()
        {
            maxHP = transform.Find("MaxHP").transform.Find("bar").GetComponent<Slider>();
            baseDamage = transform.Find("BaseDamage").transform.Find("bar").GetComponent<Slider>();
            speed = transform.Find("Speed").transform.Find("bar").GetComponent<Slider>();
            attackSpeed = transform.Find("AttackSpeed").transform.Find("bar").GetComponent<Slider>();
            leftPoints = transform.Find("LeftPoint").transform.Find("Point").GetComponent<TextMeshProUGUI>();
            lv = transform.Find("Lv").GetComponent<TextMeshProUGUI>();
            playerPreviewAnimator = transform.Find("PlayerPreview").transform.Find("Image").GetComponent<Animator>();
        }

        private void OnEnable()
        {
            var psi = PermanentStatController.Instance;
            if (playerNum == 0)
            {
                maxHP.minValue = psi.p0MinStat.maxHealth;
                baseDamage.minValue = psi.p0MinStat.baseDamage;
                speed.minValue = psi.p0MinStat.speed;
                attackSpeed.minValue = psi.p0MinStat.attackSpeed;
                maxHP.maxValue = psi.p0MaxStat.maxHealth;
                baseDamage.maxValue = psi.p0MaxStat.baseDamage;
                speed.maxValue = psi.p0MaxStat.speed;
                attackSpeed.maxValue = psi.p0MaxStat.attackSpeed;
            }
            else if (playerNum == 1)
            {
                maxHP.minValue = psi.p1MinStat.maxHealth;
                baseDamage.minValue = psi.p1MinStat.baseDamage;
                speed.minValue = psi.p1MinStat.speed;
                attackSpeed.minValue = psi.p1MinStat.attackSpeed;
                maxHP.maxValue = psi.p1MaxStat.maxHealth;
                baseDamage.maxValue = psi.p1MaxStat.baseDamage;
                speed.maxValue = psi.p1MaxStat.speed;
                attackSpeed.maxValue = psi.p1MaxStat.attackSpeed;
            }
            
            playerStat = playerNum == 0
                ? PermanentStatController.Instance.p0PermanentStats
                : PermanentStatController.Instance.p1PermanentStats;
            maxHP.value = playerStat.maxHealth;
            baseDamage.value = playerStat.baseDamage;
            speed.value = playerStat.speed;
            attackSpeed.value = playerStat.attackSpeed;
            leftPoints.text = playerNum ==0
            ? PermanentStatController.Instance.p0LeftPoints + " points"
            : PermanentStatController.Instance.p1LeftPoints + " points";
            lv.text = "Level " + (playerNum == 0
                ? PermanentStatController.Instance.p0UsedPoints
                : PermanentStatController.Instance.p1UsedPoints);
            playerPreviewAnimator.SetBool(IsMelee, playerNum == 0);
        }

               
        public void MaxHpUp()
        {
            PermanentStatController.Instance.MaxHpUp(playerNum);
            OnEnable();
        }
        
        public void BaseDamageUp()
        {
            PermanentStatController.Instance.BaseDamageUp(playerNum);
            OnEnable();
        }
        
        public void SpeedUp()
        {
            PermanentStatController.Instance.SpeedUp(playerNum);
            OnEnable();
        }
        
        public void AttackSpeedUp()
        {
            PermanentStatController.Instance.AttackSpeedUp(playerNum);
            OnEnable();
        }

        public void ResetStats()
        {
            PermanentStatController.Instance.ResetPermanentStats(playerNum);
            OnEnable();
        }
    }
}