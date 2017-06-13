using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : Collectable {

    public override void onRabbitEnter(HeroRabit rabit)
    {
        HeroRabit.current.addLife();
        this.hideCollectable();
    }

}
