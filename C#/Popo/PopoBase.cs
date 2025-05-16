using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PopoBase : MonoBehaviour
{
    public float popoSpeed = 6f;

    public float attackTimeSpace = 0.5f;

    public float damge = 1f;

    public float maxFlyDistance = 15f;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("collider");
        Debug.Log(collision.name);
        if (collision.CompareTag("Enemy"))
        {
            AttackBuff();
            collision.GetComponent<BugBase>().GetDamge(damge);
        }
    }

    public virtual void AttackBuff()
    {

    }
  

}
