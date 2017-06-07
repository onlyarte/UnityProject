using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroRabit : MonoBehaviour {

    public static HeroRabit current;

    public LivesPanel livesPanel;
    public GemPanel gemsPanel;
    public UILabel coinsLabel;
    public UILabel fruitsLabel;

    Rigidbody2D myBody = null;
    Animator myController = null;

    public float speed = 2;

    bool isGrounded = false;

    bool JumpActive = false;
    float JumpTime = 0f;
    public float MaxJumpTime = 0f;
    public float JumpSpeed = 0f;

    public int health = 1;
    public int MaxHealth = 2;
    int lives = 3;

    public int coins = 0;

    Vector3 targetScale = Vector3.one;
    Vector3 scale_speed = Vector3.one;

    void Awake()
    {
        current = this;
    }

    void Start()
    {
        myBody = this.GetComponent<Rigidbody2D>();
        myController = this.GetComponent<Animator>();
        LevelController.current.setStartPosition(this.transform.position);
        if(livesPanel != null)
            livesPanel.setLivesQuantity(lives);
        if (coinsLabel != null)
            LevelController.current.updateCoins();
    }

    public void addHealth(int number)
    {
        Debug.Log("health added");
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

    public void resetHealth()
    {
        livesPanel.setLivesQuantity(--lives);
        this.health = 1; onHealthChange();
        LevelController.current.onRabitDeath(this);
    }

    void onHealthChange()
    {
        if (this.health == 1)
        {
            targetScale = Vector3.one;
        }
        else if (this.health == 2)
        {
            targetScale = Vector3.one + new Vector3(0.5F, 0.5f, 0);
        }
        else if (this.health == 0)
        {
            StartCoroutine(rabitDie());
            livesPanel.setLivesQuantity(--lives);
        }
    }

    IEnumerator rabitDie()
    {
        this.myController.SetBool("die", true);
        yield return new WaitForSeconds(4);
        this.myController.SetBool("die", false);
        LevelController.current.onRabitDeath(this);
        this.health = 1; onHealthChange();
    }

    public bool isDead()
    {
        return this.myController.GetBool("die");
    }

    void FixedUpdate()
    {
        if (this.isDead())
            return;

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
        if (value < 0 && !animator.GetBool("die"))
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
            animator.SetBool("run", false);
        }

        this.transform.localScale = Vector3.SmoothDamp(this.transform.localScale, this.targetScale, ref scale_speed, 1.0f);
    }
}
