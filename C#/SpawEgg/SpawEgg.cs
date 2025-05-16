using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawEgg : MonoBehaviour
{
    public GameObject spawEntity;

    public float triggerDistance = 2f;

    private Collider2D player;

    //int playerMask = LayerMask.NameToLayer("Player");

    private Animator animator;
    private AnimatorStateInfo animatorStateInfo;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        animator.enabled = false;
    }

    private void Update()
    {
        if (player == null)
        {
            SearchPlayer();
        }
        else
        {
            animator.enabled = true;

            animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (animatorStateInfo.normalizedTime > 1f)
            {
                Instantiate(spawEntity, transform.position-Vector3.down*0.3f, Quaternion.identity);

                Destroy(gameObject);
            }
            
        }
    }
    private void SearchPlayer(float MaxDistance = 5f)
    {
        player = Physics2D.OverlapCircle(transform.position, MaxDistance, 1 << 3);//¼ì²âÍæ¼Ò²ã

        if (player != null)
        {
            animator.enabled = true;
        }
    }
}
