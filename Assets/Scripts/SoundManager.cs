using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    bool is_sound_on = true;
    bool is_music_on = true;
    public bool isSoundOn()
    {
        return this.is_sound_on;
    }
    public bool isMusicOn()
    {
        return this.is_music_on;
    }
    public void setSoundOn(bool val)
    {
        this.is_sound_on = val;
        PlayerPrefs.SetInt("sound", this.is_sound_on ? 1 : 0);
        PlayerPrefs.Save();
    }
    public void setMusicOn(bool val)
    {
        this.is_music_on = val;
        PlayerPrefs.SetInt("music", this.is_sound_on ? 1 : 0);
        PlayerPrefs.Save();
    }
    SoundManager()
    {
        if (PlayerPrefs.GetInt("sound", -1) == 0)
            is_sound_on = false;
        else
            is_sound_on = true;
        if (PlayerPrefs.GetInt("music", -1) == 0)
            is_music_on = false;
        else
            is_music_on = true;
    }

    public static SoundManager Instance = new SoundManager();
}
