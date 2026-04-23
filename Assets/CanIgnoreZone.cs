using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanIgnoreZone : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D colli)
    {
        if (colli.gameObject.CompareTag("CanZone"))
        {
            Physics2D.IgnoreCollision(colli.collider, GetComponent<Collider2D>());
        }
    }
}
