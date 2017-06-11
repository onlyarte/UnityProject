using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Orc1 : MonoBehaviour
{
    public static Orc1 current;
    public AudioClip attackSound = null;
    AudioSource attackSource = null;

    public enum Mode
    {
        GoToA,
        GoToB,
        Dead,
        Attack
    }

    Mode mode = Mode.GoToB;

    Rigidbody2D myBody = null;
    Animator myController = null;
    public float speed = 2;
    public float PatrolDistance = 4;
    Vector3 scale_speed;
    Vector3 targetScale = Vector3.one;

    int health = 1;

    Vector3 pointA;
    Vector3 pointB;

    public bool isDead()
    {
        return this.health == 0;
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
        if (this.health == 0)
        {
            int childCount = this.transform.childCount;
            for (int i = 0; i < childCount; i++)
                Destroy(this.transform.GetChild(i).gameObject);
            StartCoroutine(orcDie());
        }
    }

    IEnumerator orcDie()
    {
        this.mode = Mode.Dead;
        this.myController.SetTrigger("die");
        yield return new WaitForSeconds(4);
        Destroy(this.gameObject);
    }

    public void showAttack()
    {
        if (SoundManager.Instance.isSoundOn())
            attackSource.Play();
        this.myController.SetTrigger("attack");
    }

    float getDirection()
    {
        if (this.mode == Mode.Dead)
            return 0;

        Vector3 my_pos = this.transform.position;
        Vector3 rabit_pos = HeroRabit.current.transform.position;
        
        //start attack
        if (rabit_pos.x > Mathf.Min(pointA.x, pointB.x)
            && rabit_pos.x < Mathf.Max(pointA.x, pointB.x))
        {
            mode = Mode.Attack;
            current = this;
            Debug.Log("current orc set");
        }
        //end attack
        if (mode == Mode.Attack && !(rabit_pos.x > Mathf.Min(pointA.x, pointB.x)
            && rabit_pos.x < Mathf.Max(pointA.x, pointB.x)))
            mode = Mode.GoToA;

        //if attack, move to the rabbit
        if (mode == Mode.Attack && !HeroRabit.current.isDead())
        {
            if (my_pos.x < rabit_pos.x)
                return 1;
            else
                return -1;
        }

        //change mode if a or b reached
        if (this.mode == Mode.GoToB)
        {
            if (my_pos.x >= pointB.x)
                this.mode = Mode.GoToA;
        }
        else if (this.mode == Mode.GoToA)
        {
            if (my_pos.x <= pointA.x)
                this.mode = Mode.GoToB;
        }

        //get new direction
        if (this.mode == Mode.GoToB)
        {
            if (my_pos.x <= pointB.x)
                return 1;
            else
                return -1;
        }
        else if(this.mode == Mode.GoToA)
        {
            if (my_pos.x <= pointA.x)
                return 1;
            else
                return -1;
        }

        return 0;
    }

    void Start()
    {
        pointA = this.transform.position;
        pointB = pointA;

        if (PatrolDistance < 0)
        {
            pointA.x += PatrolDistance;
        }
        else
        {
            pointB.x += PatrolDistance;
        }

        myBody = this.GetComponent<Rigidbody2D>();
        myController = this.GetComponent<Animator>();
        LevelController.current.setStartPosition(this.transform.position);

        attackSource = gameObject.AddComponent<AudioSource>();
        attackSource.clip = attackSound;
    }

    void FixedUpdate()
    {
        //[-1, 1]
        float value = this.getDirection();
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
            sr.flipX = false;
        else if (value > 0)
            sr.flipX = true;
        
        this.transform.localScale = Vector3.SmoothDamp(this.transform.localScale, this.targetScale, ref scale_speed, 1.0f);
    }
}
