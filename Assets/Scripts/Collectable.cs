using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {

    public virtual void onRabbitEnter(HeroRabit rabit)
    {

    }


    void OnTriggerEnter2D(Collider2D collider)
    {
        HeroRabit rabit = collider.gameObject.GetComponent<HeroRabit>();

        if (rabit != null)
            this.onRabbitEnter(rabit);
    }
    
    public void hideCollectable()
    {
        Destroy(this.gameObject);
    }
}
