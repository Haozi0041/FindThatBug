
using UnityEngine;


public class BugBase : MonoBehaviour
{
    public float hp = 10f;
    public AnimationCurve animationCurve;
    public float speed = 3f;
    public Collider2D player;
    public float attackPower = 3f;

    private Animator animator;
    private BugState currentState;
    private int playerMask;
    public Vector2 direction = Vector2.right;//虫子moren移动方向
    private float idleTimer = 0f;//待机状态移动计数器


    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();       
        playerMask = LayerMask.NameToLayer("Player");
    }


    public void SearchPlayer(float MaxDistance = 5f)//玩家检测
    {
        player = Physics2D.OverlapCircle(transform.position, MaxDistance,1<<playerMask);//检测玩家层
       
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 5f);
    }

    public void idleMove()//待机移动模式
    {
        idleTimer += Time.fixedDeltaTime;
        
        if (idleTimer +Random.Range(0,1.5f) >3f)
        {
            direction = -direction;
            idleTimer = 0;
        }
        BugMove(direction);
    }

    public void BugMove(Vector2 direction)//BUG移动
    {        
        float mul = animationCurve.Evaluate(Time.time);
        transform.rotation = direction == Vector2.right ? Quaternion.Euler(0, 180, 0) : Quaternion.Euler(0, 0, 0);
        //Debug.Log(direction);
        transform.Translate(transform.InverseTransformDirection(direction) * Time.fixedDeltaTime * speed * mul);
        animator.SetFloat("Speed", mul);
    }

    public void GetDamge(float damge)
    {
        hp -= damge;
        Debug.Log("hurt!  Remain lives ->" + hp); 
        if (hp <= 0)
        {
            
            Die();
        }
    }
    public void Die()
    {
        Destroy(gameObject);
        Debug.Log("Enemy dead");
    }
}
