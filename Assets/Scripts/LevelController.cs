using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{

    public static LevelController current;
    void Awake()
    {
        current = this;
    }

    Vector3 startPosition;

    public void setStartPosition(Vector3 pos)
    {
        this.startPosition = pos;
    }

    public void onRabitDeath(HeroRabit rabit)
    {
        rabit.transform.position = this.startPosition;
    }

    public void addCoins(int number)
    {
        Debug.Log("coins collected " + number);
    }
}
