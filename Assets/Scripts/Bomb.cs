﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Collectable {

    public override void onRabbitEnter(HeroRabit rabit)
    {
        this.hideCollectable();
        HeroRabit.current.colideBomb();
    }
}
