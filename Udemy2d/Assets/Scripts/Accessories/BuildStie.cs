using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildStie : MonoBehaviour {

    private Transform buildSite;

    private Collider2D buildSiteCollider;
    private Collider2D site;

    public Collider2D BuildSiteCollider
    {
        get
        {
            return buildSiteCollider;
        }
    }

    public Collider2D Site
    {
        get
        {
            return site;
        }

        set
        {
            site = value;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
