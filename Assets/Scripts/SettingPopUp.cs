using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingPopUp : MonoBehaviour {
    
    public MyButton settingsCloseButton;
    public MyButton settingsCloseBackground;
    public MyButton soundButton;
    public MyButton musicButton;

    public UI2DSprite sound;
    public UI2DSprite music;

    public Sprite soundOn;
    public Sprite soundOff;
    public Sprite musicOn;
    public Sprite musicOff;

    // Use this for initialization
    void Start()
    {
        Time.timeScale = 0;
        settingsCloseButton.signalOnClick.AddListener(this.onSettingsClose);
        settingsCloseBackground.signalOnClick.AddListener(this.onSettingsClose);
        soundButton.signalOnClick.AddListener(this.onSound);
        musicButton.signalOnClick.AddListener(this.onMusic);
    }

    void Update()
    {
        if (SoundManager.Instance.isSoundOn())
            sound.sprite2D = soundOn;
        else
            sound.sprite2D = soundOff;
        if (SoundManager.Instance.isMusicOn())
            music.sprite2D = musicOn;
        else
            music.sprite2D = musicOff;
    }

    void onSettingsClose()
    {
        Time.timeScale = 1;
        Destroy(this.gameObject);
    }

    void onSound()
    {
        if (SoundManager.Instance.isSoundOn())
        {
            SoundManager.Instance.setSoundOn(false);
            sound.sprite2D = soundOff;
        }
        else
        {
            SoundManager.Instance.setSoundOn(true);
            sound.sprite2D = soundOn;
        }
    }

    void onMusic()
    {
        if (SoundManager.Instance.isMusicOn())
        {
            SoundManager.Instance.setMusicOn(false);
            music.sprite2D = musicOff;
        }
        else
        {
            SoundManager.Instance.setMusicOn(true);
            music.sprite2D = musicOn;
        }
            
    }

}
