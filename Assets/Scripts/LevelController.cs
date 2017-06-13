using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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

    public void addFruits(int id)
    {
        if (HeroRabit.current.currentStat.collectedFruits[id] > 0)
            return;
        fruits++;
        HeroRabit.current.currentStat.collectedFruits[id]++;
        HeroRabit.current.fruitsLabel.text = fruits.ToString() + "/12";
    }

    public void addGem(GemPanel.Color color)
    {
        HeroRabit.current.gemsPanel.addGem(color);
    }

    public static int getCurrentLevel(string name)
    {
        Regex re = new Regex(@"\d+");
        Match m = re.Match(name);

        if (m.Success)
        {
            return System.Int32.Parse(m.Value);
        }
        else
        {
            return -1;
        }
    }
}
