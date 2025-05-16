
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
    public Vector2 direction = Vector2.right;//����moren�ƶ�����
    private float idleTimer = 0f;//����״̬�ƶ�������


    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();       
        playerMask = LayerMask.NameToLayer("Player");
    }


    public void SearchPlayer(float MaxDistance = 5f)//��Ҽ��
    {
        player = Physics2D.OverlapCircle(transform.position, MaxDistance,1<<playerMask);//�����Ҳ�
       
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 5f);
    }

    public void idleMove()//�����ƶ�ģʽ
    {
        idleTimer += Time.fixedDeltaTime;
        
        if (idleTimer +Random.Range(0,1.5f) >3f)
        {
            direction = -direction;
            idleTimer = 0;
        }
        BugMove(direction);
    }

    public void BugMove(Vector2 direction)//BUG�ƶ�
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
