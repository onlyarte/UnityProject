using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orc2BattleController : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(Orc2.current != null && collider.gameObject.layer == 11)
            Orc2.current.removeHealth(1);
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        OnTriggerEnter2D(collider);
    }
}
