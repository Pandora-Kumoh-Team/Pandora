using Pandora.Scripts.Enemy;
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;
using UnityEngine.UIElements;

public class KnifeGenerator : MonoBehaviour
{
    [SerializeField] private GameObject leftKnife; // ������ ������Ʈ
    [SerializeField] private GameObject rightKnife;
    [SerializeField] private GameObject upKnife;
    [SerializeField] private GameObject downKnife;
    private float knifeSpeed;

    private void Start()
    {
        knifeSpeed = transform.parent.GetComponent<FirstBossController>()._enemyStatus.AttackSpeed;
    }

    public void Fire(string type)
    {
       if(type.Equals("left"))
       {
           GameObject newKnife = Instantiate(leftKnife, transform.parent.transform.position, Quaternion.identity);
           Rigidbody2D rb = newKnife.GetComponent<Rigidbody2D>();
           newKnife.GetComponent<Knife>().boss = transform.parent.gameObject;
           transform.parent.GetComponent<FirstBossController>().knifes.Add(newKnife);

           if (newKnife != null && rb != null)
           {
               rb.velocity = Vector2.left * knifeSpeed;
           }
       }else if(type.Equals("right"))
       {
           GameObject newKnife = Instantiate(rightKnife, transform.parent.transform.position, Quaternion.identity);
           Rigidbody2D rb = newKnife.GetComponent<Rigidbody2D>();
           newKnife.GetComponent<Knife>().boss = transform.parent.gameObject;
           transform.parent.GetComponent<FirstBossController>().knifes.Add(newKnife);

           if (newKnife != null && rb != null)
           {
               rb.velocity = Vector2.right * knifeSpeed;
           }
       }else if(type.Equals("up"))
       {
           GameObject newKnife = Instantiate(upKnife, transform.parent.transform.position, Quaternion.identity);
           Rigidbody2D rb = newKnife.GetComponent<Rigidbody2D>();
           newKnife.GetComponent<Knife>().boss = transform.parent.gameObject;
           transform.parent.GetComponent<FirstBossController>().knifes.Add(newKnife);

           if (newKnife != null && rb != null)
           {
               rb.velocity = Vector2.up * knifeSpeed;
           }
       }else if(type.Equals("down"))
       {
           GameObject newKnife = Instantiate(downKnife, transform.parent.transform.position, Quaternion.identity);
           Rigidbody2D rb = newKnife.GetComponent<Rigidbody2D>();
           newKnife.GetComponent<Knife>().boss = transform.parent.gameObject;
           transform.parent.GetComponent<FirstBossController>().knifes.Add(newKnife);

           if (newKnife != null && rb != null)
           {
               rb.velocity = Vector2.down * knifeSpeed;
           }
       }
    }
    
}
