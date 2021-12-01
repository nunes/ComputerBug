using System;
using UnityEngine;

public static class ColorUtil
{

    //Scale color in gameObject and children
    public static void ScaleColor(GameObject gameObject, float scale)
    {
        foreach (Renderer childRenderer in gameObject.transform.GetComponentsInChildren<Renderer>())
        {

            Color previousColor = childRenderer.material.color;

            Color color = new Color(previousColor.r * scale, previousColor.g * scale, previousColor.b * scale);

            childRenderer.material.color = color;

        }
    }

    //Set color of game object and children
    public static void SetSpriteColor(GameObject gameObject, Color newColor)
    {

        foreach (SpriteRenderer myRenderer in gameObject.GetComponents<SpriteRenderer>())
        {

            myRenderer.color = newColor;

        }

    }
}
