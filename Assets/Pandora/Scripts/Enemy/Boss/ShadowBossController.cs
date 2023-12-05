using Pandora.Scripts.Effect;
using Pandora.Scripts.Player.Controller;
using Pandora.Scripts.System.Event;
using Pandora.Scripts.System;
using Pandora.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pandora.Scripts.Enemy;

public class ShadowBossController : MonoBehaviour,IHitAble
{
    //Component
    private Rigidbody2D rb;
    public Animator anim;
    private PolygonCollider2D polygonCollider;
    private GameObject target;
    private float teleportDelay = 10f;
    private float sideAttackDelay = 15f;
    private float airAttackDelay = 25f;
    private bool onSideAttack = false;
    private bool onAirAttack = false;

    //Status
    public EnemyStatus _enemyStatus;
    private void Awake()
    {
        _enemyStatus = new EnemyStatus("3StageBoss");
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        polygonCollider = GetComponent<PolygonCollider2D>();
        StartCoroutine(Teleport());
        StartCoroutine(SideAttack());
        StartCoroutine(AirAttack());
    }

    // Update is called once per frame
    void Update()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        rb.velocity = Vector3.zero;
    }
    public void Hit(HitParams hitParams)
    {
        var damage = hitParams.damage;
        transform.Find("BossHP").gameObject.SetActive(true);
        //damage effect
        var effectPosition = transform.position + new Vector3(1.5f, 1f, 0);
        var damageEffect = Instantiate(GameManager.Instance.damageEffect, effectPosition, Quaternion.identity, transform);
        var reduceDamage = damage - (damage * _enemyStatus.DefencePower / 100);
        damageEffect.GetComponent<FadeTextEffect>()
            .Init(reduceDamage.ToString(), Color.white, 1f, 0.5f, 0.05f, Vector3.up);

        _enemyStatus.NowHealth -= reduceDamage;
        CallHealthChangeEvetnt();
        if (_enemyStatus.NowHealth <= 0)
        {
            StartCoroutine(Death());
        }
        OnHitAnimationEnd();
    }
    private void OnDisable()
    {

        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<FadeTextEffect>() != null)
                Destroy(child.gameObject);
        }
    }
    public void Attack()
    {
        target.GetComponent<PlayerController>().Hurt(_enemyStatus.BaseDamage, null, gameObject);
    }
    private void CallHealthChangeEvetnt()
    {
        var param = new BossHealthChangedParam(_enemyStatus.NowHealth, _enemyStatus.MaxHealth);
        EventManager.Instance.TriggerEvent(PandoraEventType.BossHealthChanged, param);
    }
    IEnumerator Death()
    {
        anim.SetTrigger("Death");
        yield return new WaitForSeconds(1.9f);
        Destroy(this.gameObject);
    }
    public void OnHitAnimationEnd()
    {
        anim.SetBool("isFollow", true);
    }
    IEnumerator Teleport()
    {
        yield return new WaitForSeconds(teleportDelay); //초기 시작 시 쿨타임 설정
        Vector3 newPos = transform.position;
        float x = 0;
        float y = 0;
        while (true)
        {
            int type = Random.Range(0, 2);
            Debug.Log("텔레포트 시작");
            anim.SetTrigger("Teleport");
            switch(type)
            {
                case 0:
                     x = Random.Range(-3, 3);
                     y = Random.Range(-3, 3);
                     newPos = new Vector3(target.transform.position.x + x, target.transform.position.y + y, 0); //플레이어 기준 랜덤하게
                    Debug.Log("랜덤 위치 선택");
                    break;
                case 1:
                    x = target.transform.position.x + 1f;
                    y = target.transform.position.y;
                    newPos = new Vector3(x, y, 0);
                    Debug.Log("플레이어 위치 선택");
                    break;
            }
            StartCoroutine(TeleportandAttackPattern(newPos));
            yield return new WaitForSeconds(teleportDelay);
        }
    }
    IEnumerator TeleportandAttackPattern(Vector3 newPos)
    {
        Debug.Log("텔레포트 및 공격 패턴");
        yield return new WaitForSeconds(1f); //애니메이션 끝날 때까지 1초 기다렸다가.
        transform.position = newPos; //위치 변환
        anim.SetTrigger("TeleportEnd");//텔포 종료 애니메이션
        CircleAttack();
        yield return new WaitForSeconds(1f);
    }
    public void CircleAttack()
    {
        anim.SetTrigger("CircleAttack");
        anim.SetTrigger("Idle");
    }
    public void OnCirlceAttackRange()
    {
        transform.Find("CircleAttackRange").gameObject.SetActive(true);
    }
    public void OffCirlceAttackRange()
    {
        transform.Find("CircleAttackRange").gameObject.SetActive(false);
    }
    public void CircleAttackCollisionEvent()
    {
        target.GetComponent<PlayerController>().Hurt(_enemyStatus.BaseDamage * _enemyStatus.AttackPower, null, gameObject);
    }
    public void SideAttackCollisionEvent()
    {
        target.GetComponent<PlayerController>().Hurt(_enemyStatus.BaseDamage * _enemyStatus.AttackPower, null, gameObject);
    }
    IEnumerator SideAttack()
    {
        onSideAttack = true;
        yield return new WaitForSeconds(3f);
        while (true)
        {
            yield return new WaitForSeconds(sideAttackDelay);
            anim.SetTrigger("SideAttack");
        }
    }
    public void OnSideAttackRange()
    {
        transform.Find("SideAttackRange").gameObject.SetActive(true);
    }
    public void OffSideAttackRange()
    {
        transform.Find("SideAttackRange").gameObject.SetActive(false);
    }
    public void OnAirAttack()
    {
        transform.Find("AirAttackRange").gameObject.SetActive(true);
    }
    public void OffAirAttack()
    {
        transform.Find("AirAttackRange").gameObject.SetActive(false);
    }
    public void AirAttackCollisionEvent()
    {
        target.GetComponent<PlayerController>().Hurt(_enemyStatus.BaseDamage * _enemyStatus.AttackPower + 5f, null, gameObject);
    }
    IEnumerator AirAttack()
    {
        onAirAttack = true;
        yield return new WaitForSeconds(3f);
        while(true)
        {
            yield return new WaitForSeconds(airAttackDelay);
            anim.SetTrigger("AirAttack");
        }
    }
}
