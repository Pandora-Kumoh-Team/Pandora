using Pandora.Scripts.Enemy;
using Pandora.Scripts.Player.Controller;
using System.Collections;
using UnityEngine;

namespace Pandora.Scripts.Player.Skill.SkillDetail
{
    public class SkillEventHorizon : ActiveSkill
    {
        private PlayerController _playerController;
        private float _nowDuration;
        private GameObject effect;
        private bool isActivate;
        private Vector2 currentPos;

        [Header("끌어당기는 속도")]
        public float speed;

        private float timer;

        private void Awake()
        {
            effect = transform.Find("Effect").gameObject;
            isActivate = false;
            currentPos = transform.position;
        }

        private void Update()
        {
            timer += Time.deltaTime;

            if (isActivate) 
            {
                transform.position = currentPos;
                effect.transform.localPosition = Vector3.zero;
            }

            if (_nowDuration > 0)
            {
                _nowDuration -= Time.deltaTime;
                if (_nowDuration <= 0)
                {
                    OnEndSkill();
                }
                OnDuringSkill();
            }
        }

        public override void OnUseSkill()
        {
            _nowDuration = duration;
            transform.localPosition = Vector3.zero;
            isActivate = true;
            currentPos = transform.position;

            transform.GetComponent<CircleCollider2D>().enabled = true;
            effect.SetActive(true);
        }

        public override void OnDuringSkill() { }

        public override void OnEndSkill()
        {
            isActivate = false;
            transform.GetComponent<CircleCollider2D>().enabled = false;
            effect.SetActive(false);
        }

        private void OnTriggerStay2D(Collider2D col)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                if(_nowDuration >= 0.5f)
                {
                    Vector2 direction = currentPos - (Vector2)col.transform.position;
                    direction.Normalize();
                    col.transform.GetComponent<Rigidbody2D>().velocity = direction * speed;
                }
                else
                {
                    Vector2 direction = (Vector2)col.transform.position - currentPos;
                    direction.Normalize();
                    col.transform.GetComponent<Rigidbody2D>().velocity = direction * speed;
                }

            }
        }

        private void OnTriggerExit2D(Collider2D col)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                col.transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }

        private void OnDisable()
        {
            if (_nowDuration > 0)
                OnEndSkill();
        }
    }
}