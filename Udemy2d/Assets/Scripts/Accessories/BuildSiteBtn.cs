using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSiteBtn : MonoBehaviour {

    [SerializeField] private BuildStie buildSite;
    [SerializeField] private Sprite dragSprite;
    [SerializeField] private int buildSitePrice;

    public BuildStie BuildSite
    {
        get
        {
            return buildSite;
        }
    }

    public Sprite DragSprite
    {
        get
        {
            return dragSprite;
        }
    }

    public int BuildSitePrice
    {
        get
        {
            return buildSitePrice;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
