using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatsinkScript : MonoBehaviour
{

    public Color normalColor;
    public Color cutColor;

    public bool isCut = false;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!isCut)
            {
                collision.gameObject.SendMessage("Burn");
            }
        }

    }

    public void CurrentCut()
    {
        isCut = true;
        if (cutColor != null)
        {
            ColorUtil.SetSpriteColor(gameObject, cutColor);
        }

    }
}
