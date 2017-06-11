using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orc1BattleController : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (this.gameObject.layer == 13 && Orc1.current != null 
            && !Orc1.current.isDead() && !HeroRabit.current.isDead())
        {
            Debug.Log("attack");
            Orc1.current.showAttack();
            HeroRabit.current.removeHealth(1);
        }
        else if (this.gameObject.layer == 14 && collider.gameObject.layer == 11 && HeroRabit.current != null 
            && !HeroRabit.current.isDead() && Orc1.current != null)
        {
            Orc1.current.removeHealth(1);
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        OnTriggerEnter2D(collider);
    }

}
