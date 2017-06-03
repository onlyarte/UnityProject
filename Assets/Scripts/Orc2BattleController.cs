using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orc2BattleController : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D collider)
    {
        HeroRabit.current.removeHealth(1);
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        OnTriggerEnter2D(collider);
    }
}
