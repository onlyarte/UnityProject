using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : Collectable {

    public override void onRabbitEnter(HeroRabit rabit)
    {
        //rabit.addHealth(1);
        this.hideCollectable();
    }
}
