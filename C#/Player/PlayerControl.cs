using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public Animator animator;
    public float hp = 100;
    public float maxSpeed = 4f;//最大移动速度
    public float invincibleTime = 1f;
    private float hitTimer;//受伤计时器
    public float gravity = -15f;

    public float jumpSpeed=8f;
    public float minAttackTimeSpace ;//最小武器攻击间隔有武器属性决定
    public GameObject popo;
    private bool couldBeGetDamge = true;
    private float attackTimeSpace = 0;
    private bool couldAttack = true;
    public Transform hitposition;

    //private float speed = 0f;//当前速度
    
    private Vector2 velocity;
    //private AnimatorStateInfo animatorStateInfo;
    private new Rigidbody2D rigidbody;
    private SpriteRenderer spriteRender;
    private bool isCrossing;
    public float crossCD = 3f;
    private float crossTimer;
    //private int moveState = Animator.StringToHash("WinMan_Move");
    //private int attack1State = Animator.StringToHash("WinMan_Attack1");
    //private int attack2State = Animator.StringToHash("WinMan_Attack2");

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        spriteRender = GetComponentInChildren<SpriteRenderer>();
        //rigidbody = GetComponent<Rigidbody2D>();
        
    }
    private void Start()
    {
        PlayerInit();//初始化角色信息
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Debug.Log(vertical);
        bool grounded = IsGround();
        velocity.x = horizontal * maxSpeed;
        if (horizontal != 0)//控制朝向
        {          
            transform.rotation = horizontal < 0 ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;
        }        
        animator.SetFloat("Speed", Mathf.Abs(horizontal)  * maxSpeed);   //调整动画    
        if (!grounded)
        {
            AddGravity();
            animator.SetBool("IsGround", grounded);
        }
        else
        {
            SetVelocityOfY(0);
            animator.SetBool("IsGround", grounded);
        }
        if (Input.GetButton("Jump") &&grounded==true)
        {
            if (vertical < -0.4&&crossTimer>crossCD)
            {
                CrossGround();
            }
            SetVelocityOfY(jumpSpeed);      
        }
        
        if (Input.GetButtonDown("Fire1")&&couldAttack)//攻击控制
        {
            StartCoroutine(AttackTimeSpace());           
            if (popo != null)
            {
                Instantiate(popo, hitposition.position, transform.rotation);
                Attack();
            }            
        }
        crossTimer += Time.deltaTime;
    }

    private void FixedUpdate()
    {
       
        Vector2 localVelocity = transform.InverseTransformDirection(velocity);
        transform.Translate(localVelocity * Time.fixedDeltaTime);
        
       
    }

    private bool IsGround()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, 0.1f,1<<7) ? true : false;
    }

    private void SetVelocityOfY(float speed)
    {
        velocity.y = speed;
    }

    private void AddVelocityOfY(float speed)
    {
        velocity.y += speed;
    }
    private void SetVelocityOfX(float speed)
    {
        velocity.x = speed;
    }

    private void AddVelocityOfX(float speed)
    {
        velocity.x += speed;
    }
    private void AddGravity()
    {
        float mul = velocity.y<0?2f:1.5f;
        
        velocity.y += gravity * Time.deltaTime * mul;
    }
 

    private void Attack()
    {
        //Debug.Log("Damege!");
    }

    private void PlayerInit()
    {
        popo = Resources.Load<GameObject>("Popo/Popo_Green");
        if (popo == null)
        {
            Debug.Log("MULL");
        }
        minAttackTimeSpace = popo.GetComponent<PopoBase>().attackTimeSpace;
    }

    public void GetDamge(float damge,Vector2 hitDirection)
    {       
        if (couldBeGetDamge)
        {
            StartCoroutine(HitTimeSpace());
            StartCoroutine(HitAnimation());
            hp -= damge;
            Debug.Log("GetDamge-->" + damge);
        }       
    }

    private IEnumerator HitAnimation()
    {
        float t = 0;
        float duration = 0;
        Color color1 = new Color(1,1,1,0.1f);
        Color color2 = new Color(1,1,1,1);
        while (duration < 0.75f)
        {
            t += Time.deltaTime;
            duration += Time.deltaTime;
            if (t > 0.1f)
            {
                t = 0;
                spriteRender.color = spriteRender.color == color1 ? color2 : color1;
                
            }
            yield return null;           
        }
        spriteRender.color = color2;
    }
    private IEnumerator AttackTimeSpace()
    {
        while (attackTimeSpace < minAttackTimeSpace)
        {
            couldAttack = false;
            attackTimeSpace += Time.deltaTime;
            yield return null;
        }
        attackTimeSpace = 0;
        couldAttack = true;
    }
    private IEnumerator HitTimeSpace()
    {
        while (hitTimer < invincibleTime)
        {
            couldBeGetDamge = false;
            hitTimer += Time.deltaTime;
            yield return null;
        }
        hitTimer = 0;
        couldBeGetDamge = true;
    }

    private void CrossGround()
    {      
        transform.position += Vector3.down * 2;
        crossTimer = 0;
        isCrossing = false;
    }

   
}
