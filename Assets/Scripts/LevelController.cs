using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{

    public static LevelController current;
    public static int coins;
    public int fruits = 0;

    void Awake()
    {
        current = this;
        coins = PlayerPrefs.GetInt("coins", 0);
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
        coins += number;
        updateCoins();
    }

    public void updateCoins()
    {
        string s = coins.ToString();
        s = s.PadLeft(4, '0');
        HeroRabit.current.coinsLabel.text = s;
    }

    public void addFruits(Fruit.FruitType type)
    {
        fruits++;
        HeroRabit.current.currentStat.collectedFruits[(int)type]++;
        HeroRabit.current.fruitsLabel.text = fruits.ToString() + "/12";
    }

    public void addGem(GemPanel.Color color)
    {
        HeroRabit.current.gemsPanel.addGem(color);
    }
}
