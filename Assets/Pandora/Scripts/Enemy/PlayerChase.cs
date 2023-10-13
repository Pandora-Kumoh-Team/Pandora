using Pandora.Scripts.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChase : MonoBehaviour
{
    // Components
    public float speed = 0.1f; //юс╫ц
    private Vector3 direction;
    private GameObject target;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // find player
        if (collision.gameObject.tag == "Player" && target == null)
        {
            target = collision.gameObject;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (target == collision.gameObject)
        {
            direction = target.transform.position - transform.parent.position;
            direction.Normalize();
            transform.parent.position += direction * speed * Time.deltaTime;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (target == collision.gameObject)
        {
            target = null;
        }
    }
}
