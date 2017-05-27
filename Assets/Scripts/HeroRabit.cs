using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroRabit : MonoBehaviour {

    public static HeroRabit current;

    Rigidbody2D myBody = null;
    Animator myController = null;

    public float speed = 2;

    bool isGrounded = false;

    bool JumpActive = false;
    float JumpTime = 0f;
    public float MaxJumpTime = 0f;
    public float JumpSpeed = 0f;

    Transform rabbitParent = null;

    public int health = 2;
    public int MaxHealth = 2;
    
    bool isSuper = false;
    bool colidedBomb = false;

    public float WaitTime = 2f;
    float to_wait = 0f;

    /*private Vector3 scale_speed;
    private Vector3 targetScale;*/

    void Awake()
    {
        current = this;
        to_wait = WaitTime;
    }

    void Start()
    {
        myBody = this.GetComponent<Rigidbody2D>();
        myController = this.GetComponent<Animator>();
        rabbitParent = this.transform.parent;
        LevelController.current.setStartPosition(this.transform.position);
    }

    public void addHealth(int number)
    {
        this.health += number;
        if (this.health > MaxHealth)
            this.health = MaxHealth;
        this.onHealthChange();
    }

    public void removeHealth(int number)
    {
        this.health -= number;
        if (this.health < 0)
            this.health = 0;
        this.onHealthChange();
    }

    void onHealthChange()
    {
        if (this.health == 1)
        {
            this.transform.localScale = Vector3.one;
        }
        else if (this.health == 2)
        {
            this.transform.localScale = Vector3.one * 2;
        }
        else if (this.health == 0)
        {
            LevelController.current.onRabitDeath(this);
        }
    }

    public void becomeSuper()
    {
        if (!isSuper)
        {
            isSuper = true;
            transform.localScale += new Vector3(0.5F, 0.5f, 0);
        }
    }

    public void colideBomb()
    {
        Animator animator = GetComponent<Animator>();
        if (isSuper)
        {
            isSuper = false;
            transform.localScale += new Vector3(-0.5F, -0.5f, 0);
        }
        else
        {
            colidedBomb = true;
            animator.SetBool("dead", true);
        }
    }

    void FixedUpdate()
    {
        //[-1, 1]
        float value = Input.GetAxis("Horizontal");
        Animator animator = GetComponent<Animator>();
        if (Mathf.Abs(value) > 0)
        {
            animator.SetBool("run", true);
            Vector2 vel = myBody.velocity;
            vel.x = value * speed;
            myBody.velocity = vel;
        }
        else
            animator.SetBool("run", false);

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (value < 0)
            sr.flipX = true;
        else if (value > 0)
            sr.flipX = false;

        Vector3 from = transform.position + Vector3.up * 0.3f;
        Vector3 to = transform.position + Vector3.down * 0.1f;
        int layer_id = 1 << LayerMask.NameToLayer("Ground");

        //Перевіряємо чи проходить лінія через Collider з шаром Ground
        RaycastHit2D hit = Physics2D.Linecast(from, to, layer_id);

        if (hit)
        {
            if (hit.transform != null && hit.transform.GetComponent<MovingPlatform>() != null)
                this.transform.parent = hit.transform;
            else
                this.transform.parent = null;

            isGrounded = true;
        }
        else
        {
            isGrounded = false;
            this.transform.parent = null;
        }

        if (this.isGrounded)
        {
            myController.SetBool("jump", false);
        }
        else
        {
            myController.SetBool("jump", true);
        }

        //Якщо кнопка тільки що натислась
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            this.JumpActive = true;
        }
        if (this.JumpActive)
        {
            //Якщо кнопку ще тримають
            if (Input.GetButton("Jump"))
            {
                this.JumpTime += Time.deltaTime;
                if (this.JumpTime < this.MaxJumpTime)
                {
                    Vector2 vel = myBody.velocity;
                    vel.y = JumpSpeed * (1.0f - JumpTime / MaxJumpTime);
                    myBody.velocity = vel;
                }
            }
            else
            {
                this.JumpActive = false;
                this.JumpTime = 0;
            }
        }

        if (this.isGrounded)
        {
            animator.SetBool("jump", false);
        }
        else
        {
            animator.SetBool("jump", true);
        }
        
        if (colidedBomb)
        {
            to_wait -= Time.deltaTime;
            if(to_wait <= 0)
            {
                colidedBomb = false;
                animator.SetBool("dead", false);
                LevelController.current.onRabitDeath(this);
                to_wait = WaitTime;
            }
        }

        //this.transform.localScale = Vector3.SmoothDamp(this.transform.localScale, this.targetScale, ref scale_speed, 1.0f);
    }
}
