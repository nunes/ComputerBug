using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitScript : MonoBehaviour
{
    public bool exitEnabled = false;


    public Color colorEnabled;
    public Color colorDisabled;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (exitEnabled)
            {
                GameObjectUtil.GetGameManager().NextLevel();
            }

        }

    }

    public void EnableExit()
    {
        this.exitEnabled = true;
        ColorUtil.SetSpriteColor(gameObject, colorEnabled);
    }

}