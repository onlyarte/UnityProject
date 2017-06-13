using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : Collectable {

    public int id;

    void Update()
    {
        if(HeroRabit.current.currentStat.collectedFruits[id] > 0)
        {
            this.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        }
    }

    public override void onRabbitEnter(HeroRabit rabit)
    {
        LevelController.current.addFruits(id);
        this.hideCollectable();
    }
}
