using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBugControl : BugBase
{
    public float attackTimeSpace = 1f;
    public float maxDistance = 4.5f;

    private float attackTimer = 0f;//ÄÚÖÃ¼ÆÊ±Æ÷
    private bool isAttack;
    private bool couldAttack;
    private float searchTimer;

    private void Update()
    {
        searchTimer += Time.deltaTime;
        if (searchTimer > 1.2f)
        {
            searchTimer = 0;
        }
        SearchPlayer();
        isAttack = player != null;

    }
    private void FixedUpdate()
    {
        if (isAttack)
        {
            AttackMove();
        }
        else
        {
            idleMove();
        }
    }

    private void AttackMove()//¹¥»÷ÒÆ¶¯
    {
        if (player != null)
        {
            if (searchTimer > 0.8f)
            {
                direction = new Vector2(player.transform.position.x - transform.position.x, 0).normalized;
            }
            BugMove(direction);
            attackTimer += Time.fixedDeltaTime;
            if (attackTimer > attackTimeSpace)
            {
                couldAttack = true;
            }
            else
            {
                couldAttack = false;
            }

            Attack(couldAttack, direction);
        }
    }
    private void Attack(bool couldAttack, Vector2 hitDirection)
    {
        //Debug.Log(Vector3.Distance(player.transform.position, transform.position));
        if (couldAttack && Vector3.Distance(player.transform.position, transform.position) < 1f)
        {
            player.transform.GetComponent<PlayerControl>().GetDamge(attackPower, hitDirection);
            attackTimer = 0;//Çå¿Õ¹¥»÷ÀäÈ´CD
        }
    }
}