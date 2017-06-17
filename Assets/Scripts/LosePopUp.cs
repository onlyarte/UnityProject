using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LosePopUp : MonoBehaviour {

    public MyButton closeButton;
    public MyButton closeBackground;
    public MyButton menuButton;
    public MyButton repeatButton;

    public List<UI2DSprite> gems;
    public Sprite gemEmpty;
    public List<Sprite> gemColorful;

    public AudioClip loseSound = null;
    AudioSource loseSource = null;

    // Use this for initialization
    void Start()
    {
        closeButton.signalOnClick.AddListener(this.openMenu);
        closeBackground.signalOnClick.AddListener(this.openMenu);
        menuButton.signalOnClick.AddListener(this.openMenu);
        repeatButton.signalOnClick.AddListener(this.repeat);
        
        loseSource = gameObject.AddComponent<AudioSource>();
        loseSource.clip = loseSound;
        loseSource.Play();

        for (int i = 0; i < 3; i++)
        {
            if (GemPanel.current.map.ContainsKey((GemPanel.Color)i)
            && GemPanel.current.map[(GemPanel.Color)i])
                gems[i].sprite2D = gemColorful[i];
            else
                gems[i].sprite2D = gemEmpty;
        }

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
