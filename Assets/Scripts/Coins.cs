using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : Collectable {

	public override void onRabbitEnter(HeroRabit rabit)
    {
        LevelController.current.addCoins(1);
        this.hideCollectable();
    }
}
