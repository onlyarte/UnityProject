using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LivesPanel : MonoBehaviour {

    public List<UI2DSprite> hearts;

    public Sprite spriteFull;
    public Sprite spriteEmpty;

    public GameObject losePrefab;

    public void setLivesQuantity(int lives)
    {
        if(lives == 0)
        {
            GameObject parent = UICamera.first.transform.parent.gameObject;
            GameObject obj = NGUITools.AddChild(parent, losePrefab);
        }

        for (int i = 0; i < 3; ++i)
        {
            if (i < lives)
                hearts[i].sprite2D = this.spriteFull;
            else
                hearts[i].sprite2D = this.spriteEmpty;
        }
    }
}
