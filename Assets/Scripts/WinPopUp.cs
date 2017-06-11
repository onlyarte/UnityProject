using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinPopUp : MonoBehaviour {

    public MyButton closeButton;
    public MyButton closeBackground;
    public MyButton menuButton;
    public MyButton repeatButton;

    public List<UI2DSprite> gems;
    public Sprite gemEmpty;
    public List<Sprite> gemColorful;

    public UILabel coinsLabel;
    public UILabel fruitsLabel;

    public Dictionary<int, LevelStat> fullStats;

    // Use this for initialization
    void Start()
    {
        closeButton.signalOnClick.AddListener(this.openMenu);
        closeBackground.signalOnClick.AddListener(this.openMenu);
        menuButton.signalOnClick.AddListener(this.openMenu);
        repeatButton.signalOnClick.AddListener(this.repeat);
        
        LevelStat stat = HeroRabit.current.currentStat;
        //show statistics
        stat.levelPassed = true;
        if (stat.collectedFruits[0] > 0 && stat.collectedFruits[1] > 0 && stat.collectedFruits[2] > 0)
            stat.hasAllFruits = true;
        int newCoins = LevelController.coins - PlayerPrefs.GetInt("coins", 0);
        coinsLabel.text = "+" + newCoins;
        fruitsLabel.text = LevelController.current.fruits.ToString() + "/12";
        for(int i = 0; i < 3; i++)
        {
            if (GemPanel.current.map.ContainsKey((GemPanel.Color)i)
            && GemPanel.current.map[(GemPanel.Color)i])
                gems[i].sprite2D = gemColorful[i];
            else
                gems[i].sprite2D = gemEmpty;
        }
        //save statistics
        PlayerPrefs.SetInt("coins", LevelController.coins);

        string input = PlayerPrefs.GetString("stats", null);
        if (input != null)
            fullStats = JsonUtility.FromJson<Dictionary<int, LevelStat>>(input);
        fullStats = new Dictionary<int, LevelStat>();

        fullStats[DoorController.current.level] = stat;

        string output = JsonUtility.ToJson(fullStats);
        PlayerPrefs.SetString("stats", output);

        HeroRabit.current.locked = true;
    }

    void openMenu()
    {
        SceneManager.LoadScene("ChoseLevel");
    }

    void repeat()
    {
        SceneManager.LoadScene("Level" + DoorController.current.level);
    }
}
