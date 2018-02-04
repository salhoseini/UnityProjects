using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccessoriesBtn : MonoBehaviour {

    [SerializeField] private Accessory accessory;
    [SerializeField] private Sprite dragSprite;
    [SerializeField] private int accessoryPrice;

    public Accessory Accessory
    {
        get
        {
            return accessory;
        }

        set
        {
            accessory = value;
        }
    }

    public Sprite DragSprite
    {
        get
        {
            return dragSprite;
        }

        set
        {
            dragSprite = value;
        }
    }

    public int AccessoryPrice
    {
        get
        {
            return accessoryPrice;
        }

        set
        {
            accessoryPrice = value;
        }
    }

}
