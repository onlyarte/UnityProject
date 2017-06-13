using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour {

    public static DoorController current;
    static int lastReached = 0;

    public int doorNumber = 0;
    public int level = 0;

    public UI2DSprite gem;
    public Sprite gemEmpty;
    public Sprite gemFull;
    public UI2DSprite fruit;
    public Sprite fruitEmpty;
    public Sprite fruitFull;

    public UI2DSprite passed;
    public Sprite passedSprite;

    public GameObject winPrefab;

    void Awake()
    {
        current = this;
    }

    void Start()
    {
        if (gem == null)
            return;

        string input = PlayerPrefs.GetString("stats" + doorNumber, null);
        LevelStat stats = JsonUtility.FromJson<LevelStat>(input);
        if(stats == null)
        {
            gem.sprite2D = gemEmpty;
            fruit.sprite2D = fruitEmpty;
            string prevInput = PlayerPrefs.GetString("stats" + doorNumber, null);
        }
        else
        {
            if (stats.levelPassed && doorNumber > lastReached)
                lastReached = doorNumber;
            if (stats.hasGems)
                gem.sprite2D = gemFull;
            else
                gem.sprite2D = gemEmpty;

            if (stats.hasAllFruits)
                fruit.sprite2D = fruitFull;
            else
                fruit.sprite2D = fruitEmpty;

            if (stats.levelPassed)
                passed.sprite2D = passedSprite;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (doorNumber > lastReached + 1)
            return;
        HeroRabit rabit = collider.GetComponent<HeroRabit>();
        if (rabit != null)
        {
            if(doorNumber != 0)
                SceneManager.LoadScene("Level" + doorNumber);
            else
            {
                GameObject parent = UICamera.first.transform.parent.gameObject;
                GameObject obj = NGUITools.AddChild(parent, winPrefab);
            }
        }
    }
}
