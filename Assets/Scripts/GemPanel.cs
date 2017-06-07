using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemPanel : MonoBehaviour {

	public enum Color
    {
        Blue=0,
        Red,
        Green,
    }

    public List<UI2DSprite> gems;

    public Sprite gemEmpty;
    public List<Sprite> gemColorful;
    public Dictionary<Color, bool> map = new Dictionary<Color, bool>();

    void Start()
    {
        for (int i = 0; i < 3; ++i)
            gems[i].sprite2D = this.gemEmpty;
    }

    public void addGem(Color color)
    {
        Debug.Log("working");
        if (!map.ContainsKey(color))
        {
            map[color] = true;
            gems[map.Count - 1].sprite2D = gemColorful[(int)color];
        }
    }
}
