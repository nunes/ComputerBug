using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableScript : MonoBehaviour
{

    public GameObject targetComponent;


    public Color onColor;

    public Color cutColor;

    public void Cut()
    {
        ColorUtil.SetSpriteColor(gameObject, cutColor);

        if (targetComponent != null)
        {
            targetComponent.SendMessage("CurrentCut");
        }

    }

}
