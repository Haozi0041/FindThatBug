using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluePopoControl : PopoBase
{
    private float timer;
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= maxFlyDistance / popoSpeed)
        {
            Destroy(gameObject);
        }
        transform.Translate(Vector2.right * popoSpeed * Time.deltaTime);
    }
    public override void AttackBuff()
    {
        Destroy(gameObject);
    }
}
