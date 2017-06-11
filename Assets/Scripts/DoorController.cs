using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour {

    public static DoorController current;

    public int doorNumber = 0;
    public int level = 0;

    public GameObject winPrefab;

    void OnTriggerEnter2D(Collider2D collider)
    {
        HeroRabit rabit = collider.GetComponent<HeroRabit>();
        if (rabit != null)
        {
            if(doorNumber != 0)
                SceneManager.LoadScene("Level" + doorNumber);
            else if(level != 0)
            {
                current = this;
                GameObject parent = UICamera.first.transform.parent.gameObject;
                GameObject obj = NGUITools.AddChild(parent, winPrefab);
            }
        }
    }
}
