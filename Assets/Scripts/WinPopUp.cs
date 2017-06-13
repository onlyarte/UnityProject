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

    public AudioClip winSound = null;
    AudioSource winSource = null;

    // Use this for initialization
    void Start()
    {
        closeButton.signalOnClick.AddListener(this.openMenu);
        closeBackground.signalOnClick.AddListener(this.openMenu);
        menuButton.signalOnClick.AddListener(this.openMenu);
        repeatButton.signalOnClick.AddListener(this.repeat);

        winSource = gameObject.AddComponent<AudioSource>();
        winSource.clip = winSound;
        winSource.Play();

        LevelStat stat = HeroRabit.current.currentStat;
        //show statistics
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
        int level = LevelController.getCurrentLevel(SceneManager.GetActiveScene().name);
        PlayerPrefs.SetInt("coins", LevelController.coins);

        stat.levelPassed = true;
        for(int i = 0; i < 12; i++)
        {
            if (stat.collectedFruits[i] == 0)
            {
                stat.hasAllFruits = false;
                break;
            }
            stat.hasAllFruits = true;
        }

        string output = JsonUtility.ToJson(stat);
        PlayerPrefs.SetString("stats" + level, output);
        Debug.Log(PlayerPrefs.GetString("stats" + 1, null));
        Debug.Log(DoorController.current.level);

        HeroRabit.current.locked = true;
    }

    void openMenu()
    {
        SceneManager.LoadScene("ChoseLevel");
    }

    void repeat()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
