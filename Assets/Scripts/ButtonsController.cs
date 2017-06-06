using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsController : MonoBehaviour {

    public MyButton menuPlayButton;
    public MyButton settingsButton;
    public MyButton settingsCloseButton;
    public MyButton settingsCloseBackground;
    public MyButton soundButton;

	// Use this for initialization
	void Start () {
        if(menuPlayButton != null)
            menuPlayButton.signalOnClick.AddListener(this.onPlay);
        if (settingsButton != null)
            settingsButton.signalOnClick.AddListener(this.onSettings);
        if (settingsCloseButton != null)
            settingsCloseButton.signalOnClick.AddListener(this.onSettingsClose);
        if (settingsCloseBackground != null)
            settingsCloseBackground.signalOnClick.AddListener(this.onSettingsClose);
        if (soundButton != null)
            soundButton.signalOnClick.AddListener(this.onSound);
	}
	
	void onPlay()
    {
        SceneManager.LoadScene("ChoseLevel");
    }

    void onSettings()
    {

    }

    void onSettingsClose()
    {

    }

    void onSound()
    {

    }
}
