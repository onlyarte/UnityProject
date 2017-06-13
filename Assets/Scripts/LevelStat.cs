using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelStat
{
    public bool hasGems = false;
    public bool hasAllFruits = false;
    public bool levelPassed = false;
    public List<int> collectedFruits = new List<int>();

    public void setCollectedFruits(List<int> prev)
    {
        collectedFruits = prev;
    }
}
