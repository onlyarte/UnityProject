using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsController : MonoBehaviour {

    public MyButton menuPlayButton;
    public MyButton settingsButton;

    public GameObject settingsPrefab;

	// Use this for initialization
	void Start () {
        if(menuPlayButton != null)
            menuPlayButton.signalOnClick.AddListener(this.onPlay);
        if (settingsButton != null)
            settingsButton.signalOnClick.AddListener(this.onSettings);
    }

    void onPlay()
    {
        SceneManager.LoadScene("ChoseLevel");
    }

    void onSettings()
    {
        GameObject parent = UICamera.first.transform.parent.gameObject;
        GameObject obj = NGUITools.AddChild(parent, settingsPrefab);
    }

}
