using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrot : Collectable {

    public override void onRabbitEnter(HeroRabit rabit)
    {
        rabit.removeHealth(1);
        this.hideCollectable();
    }

    float my_direction = 0;
    public float Speed = 0.001f;

    public void launch(float direction)
    {
        this.my_direction = direction;

        if(direction < 0)
        {
            this.GetComponent<SpriteRenderer>().flipX = true;
        }

        StartCoroutine(destroyLater());
    }

    IEnumerator destroyLater()
    {
        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject);
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 pos = this.transform.position;
        pos.x += Time.deltaTime + my_direction*0.1f;
        this.transform.position = pos;
	}
}
