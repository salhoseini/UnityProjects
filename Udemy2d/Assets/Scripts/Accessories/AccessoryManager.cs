using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AccessoryManager : MonoBehaviour {

    public static AccessoryManager instance;

    public AccessoriesBtn selectedAccessory;
    private SpriteRenderer spriteRenderer;
    private Collider2D buildSite;
    private List<Accessory> accessories;

    public List<Accessory> Accessories
    {
        get
        {
            return accessories;
        }
    }

    public static AccessoryManager getInstance()
    {
        if (instance == null)
        {
            instance = new AccessoryManager();
        }
        return instance;
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }



    // Use this for initialization
    void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        buildSite = GetComponent<Collider2D>();
        accessories = new List<Accessory>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
            if (hit.collider.tag == "path")
            {
                buildSite = hit.collider;
                buildSite.tag = "pathFull";
                placeAccessory(hit);
            }
        }

        if (spriteRenderer.enabled)
        {
            followMouse();
        }
    }

    public void placeAccessory(RaycastHit2D hit)
    {
        if (!EventSystem.current.IsPointerOverGameObject() && selectedAccessory != null)
        {
            Accessory newAccessory = Instantiate(selectedAccessory.Accessory);
            newAccessory.transform.position = hit.transform.position;
            newAccessory.Site = hit.collider;
            //registerTower(newTower);
            Accessories.Add(newAccessory);
            disableDragSprite();
            selectedAccessory = null;
        }
    }

    public void selectAccessory(AccessoriesBtn accessory)
    {
        selectedAccessory = accessory;
        if (selectedAccessory.AccessoryPrice <= GameManager.getInstance().TotalMoney)
        {
            buyAccessory(selectedAccessory.AccessoryPrice);
            enableDragSprite(selectedAccessory.DragSprite);
        }

    }

    public void buyAccessory(int price)
    {
        GameManager.getInstance().subtractMoney(price);
    }

    public void followMouse()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(transform.position.x, transform.position.y);
    }

    public void enableDragSprite(Sprite sprite)
    {
        spriteRenderer.enabled = true;
        spriteRenderer.sprite = sprite;
    }

    public void disableDragSprite()
    {
        spriteRenderer.enabled = false;
    }

    public void destroyAccessory(Accessory accessory)
    {
        {
            // play destroy audio            
            accessory.IsDestroyed = true;
            accessory.Site.tag = "path";
            accessory.gameObject.SetActive(false);
            Destroy(accessory.AccessoryCollider);
        }
    }

    public void resetAccessoriesList()
    {
        accessories = new List<Accessory>();
        foreach(Accessory accessory in accessories)
        {
            if(accessory.IsDestroyed)
            {
                Destroy(accessory.gameObject);
            }
            else
            {
                accessories.Add(accessory);
            }
        }
    }
}
