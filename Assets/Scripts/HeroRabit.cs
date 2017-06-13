using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HeroRabit : MonoBehaviour {

    public static HeroRabit current;

    public LivesPanel livesPanel;
    public GemPanel gemsPanel;
    public UILabel coinsLabel;
    public UILabel fruitsLabel;

    public LevelStat currentStat;

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

    public AudioClip runSound = null;
    AudioSource runSource = null;
    public AudioClip dieSound = null;
    AudioSource dieSource = null;
    public AudioClip groundSound = null;
    AudioSource groundSource = null;
    public AudioClip backgroundSound = null;
    public AudioSource backgroundSource = null;

    public bool locked = false;

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
        
        currentStat = new LevelStat();
        for (int i = 0; i < 12; i++)
            currentStat.collectedFruits.Add(0);

        int level = LevelController.getCurrentLevel(SceneManager.GetActiveScene().name);
        if(level > 0)
        {
            /*PlayerPrefs.SetString("stats1", "");
            PlayerPrefs.SetString("stats2", "");
            return;*/
            string input = PlayerPrefs.GetString("stats" + level, null);
            LevelStat archStat = JsonUtility.FromJson<LevelStat>(input);

            if (archStat != null && archStat.collectedFruits.Count == 12)
            {
                for (int i = 0; i < 12; i++)
                {
                    if (archStat.collectedFruits[i] > 0)
                    {
                        currentStat.collectedFruits[i] = 1;
                        LevelController.current.fruits++;
                    }
                }
                if (archStat.hasGems)
                    currentStat.hasGems = true;
            }

            fruitsLabel.text = LevelController.current.fruits.ToString() + "/12";
        }

        runSource = gameObject.AddComponent<AudioSource>();
        runSource.clip = runSound;
        runSource.loop = true;
        dieSource = gameObject.AddComponent<AudioSource>();
        dieSource.clip = dieSound;
        groundSource = gameObject.AddComponent<AudioSource>();
        groundSource.clip = groundSound;
        backgroundSource = gameObject.AddComponent<AudioSource>();
        backgroundSource.clip = backgroundSound;
        backgroundSource.loop = true;
        if (SoundManager.Instance.isMusicOn())
            backgroundSource.Play();
    }

    public void addHealth(int number)
    {
        Debug.Log("health added");
        this.health += number;
        if (this.health > MaxHealth)
            this.health = MaxHealth;
        this.onHealthChange();
    }

    public void addLife()
    {
        if(lives < 3)
            livesPanel.setLivesQuantity(++lives);
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
        if (locked)
            return;
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
            if (SoundManager.Instance.isSoundOn())
                dieSource.Play();
            livesPanel.setLivesQuantity(--lives);
            StartCoroutine(rabitDie());
        }
    }

    IEnumerator rabitDie()
    {
        this.myController.SetBool("die", true);
        yield return new WaitForSeconds(4);
        if (lives > 0)
        {
            this.myController.SetBool("die", false);
            LevelController.current.onRabitDeath(this);
            this.health = 1; onHealthChange();
        }
    }

    public bool isDead()
    {
        return this.myController.GetBool("die");
    }

    void FixedUpdate()
    {
        if (SoundManager.Instance.isMusicOn() && !backgroundSource.isPlaying)
            backgroundSource.Play();
        else if (!SoundManager.Instance.isMusicOn() && backgroundSource.isPlaying)
            backgroundSource.Stop();

        if (this.isDead() || locked)
            return;

        //[-1, 1]
        float value = Input.GetAxis("Horizontal");
        Animator animator = GetComponent<Animator>();
        if (Mathf.Abs(value) > 0)
        {
            animator.SetBool("run", true);
            if (SoundManager.Instance.isSoundOn())
                runSource.Play();
            Vector2 vel = myBody.velocity;
            vel.x = value * speed;
            myBody.velocity = vel;
        }
        else
        {
            animator.SetBool("run", false);
            if (SoundManager.Instance.isSoundOn())
                runSource.Stop();
        }

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
            if (SoundManager.Instance.isSoundOn())
                groundSource.Play();
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
