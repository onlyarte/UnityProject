using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Orc2 : MonoBehaviour
{
    public static Orc2 current;

    public enum Mode
    {
        GoToA,
        GoToB,
        Dead,
        CarrotLeft,
        CarrotRight,
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

    public GameObject carrot;
    float last_carrot = 0;

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

    float getDirection()
    {
        if (this.mode == Mode.Dead)
            return 0;
        if (this.mode == Mode.CarrotLeft)
            return -0.0000001f;
        if (this.mode == Mode.CarrotRight)
            return 0.0000001f;

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
        else if (this.mode == Mode.GoToA)
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
    }

    void FixedUpdate()
    {
        //[-1, 1]
        float value = this.getDirection();
        Animator animator = GetComponent<Animator>();

        if (this.mode == Mode.Attack)
        {
            animator.SetBool("run", false);
            launchCarrot();
        }
        else if (Mathf.Abs(value) > 0)
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

    void launchCarrot()
    {
        if (this.health > 0)
        {
            Vector3 my_pos = this.transform.position;
            Vector3 rabit_pos = HeroRabit.current.transform.position;

            if (Mathf.Abs(my_pos.x - rabit_pos.x) <= 7.0f)
            {
                if (Time.time - this.last_carrot > 4.0f && !HeroRabit.current.isDead())
                {
                    this.last_carrot = Time.time;

                    GameObject obj = GameObject.Instantiate(this.carrot);
                    obj.transform.position = my_pos + Vector3.up * 0.5f;

                    Carrot carrot = obj.GetComponent<Carrot>();
                    if (rabit_pos.x < my_pos.x)
                        carrot.launch(-1);
                    else
                        carrot.launch(1);
                }
            }
        }
    }
}
