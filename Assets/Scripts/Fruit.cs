using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : Collectable {

    public enum FruitType
    {
        Apple = 0,
        Grapes,
        Cherry,
    }

    public FruitType type;

    public override void onRabbitEnter(HeroRabit rabit)
    {
        LevelController.current.addFruits(type);
        this.hideCollectable();
    }
}
