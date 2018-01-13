using UnityEngine;
using System.Collections;

public class TowerController : MonoBehaviour {

    [SerializeField] private GameObject towerObject;
    [SerializeField] private Sprite dragSprite;

	public GameObject TowerObject {
		get{ 
			return towerObject;
		}
	}

    public Sprite DragSprite
    {
        get
        {
            return dragSprite;
        }
    }
}
