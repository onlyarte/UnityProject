using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : Collectable {

    public override void onRabbitEnter(HeroRabit rabit)
    {
        HeroRabit.current.becomeSuper();
        this.hideCollectable();
    }
}
