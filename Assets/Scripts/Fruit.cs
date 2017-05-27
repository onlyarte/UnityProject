using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : Collectable {

    public override void onRabbitEnter(HeroRabit rabit)
    {
        //rabit.addHealth(1);
        this.hideCollectable();
    }
}
