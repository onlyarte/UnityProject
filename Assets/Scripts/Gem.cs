using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : Collectable {

    public GemPanel.Color color;

    public override void onRabbitEnter(HeroRabit rabit)
    {
        LevelController.current.addGem(color);
        this.hideCollectable();
    }
}
