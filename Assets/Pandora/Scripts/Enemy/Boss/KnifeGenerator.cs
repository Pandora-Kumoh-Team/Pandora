using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;
using UnityEngine.UIElements;

public class KnifeGenerator : MonoBehaviour
{
    [SerializeField] private GameObject leftKnife; // 나이프 오브젝트
    [SerializeField] private GameObject rightKnife;
    [SerializeField] private GameObject upKnife;
    [SerializeField] private GameObject downKnife;
    public float knifeSpeed = 5.0f; //임시

    public void Fire(string type)
    {
       if(type.Equals("left"))
        {
            GameObject newKnife = Instantiate(leftKnife, transform.parent.transform.position, Quaternion.identity);
            Rigidbody2D rb = newKnife.GetComponent<Rigidbody2D>();

            if (newKnife != null && rb != null)
            {
                rb.velocity = Vector2.left * knifeSpeed;
            }
        }else if(type.Equals("right"))
        {
            GameObject newKnife = Instantiate(rightKnife, transform.parent.transform.position, Quaternion.identity);
            Rigidbody2D rb = newKnife.GetComponent<Rigidbody2D>();

            if (newKnife != null && rb != null)
            {
                rb.velocity = Vector2.right * knifeSpeed;
            }
        }else if(type.Equals("up"))
        {
            GameObject newKnife = Instantiate(upKnife, transform.parent.transform.position, Quaternion.identity);
            Rigidbody2D rb = newKnife.GetComponent<Rigidbody2D>();

            if (newKnife != null && rb != null)
            {
                rb.velocity = Vector2.up * knifeSpeed;
            }
        }else if(type.Equals("down"))
        {
            GameObject newKnife = Instantiate(downKnife, transform.parent.transform.position, Quaternion.identity);
            Rigidbody2D rb = newKnife.GetComponent<Rigidbody2D>();

            if (newKnife != null && rb != null)
            {
                rb.velocity = Vector2.down * knifeSpeed;
            }
        }
    }
    
}
