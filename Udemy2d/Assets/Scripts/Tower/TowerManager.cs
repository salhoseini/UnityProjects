using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class TowerManager : MonoBehaviour
{

    public static TowerManager instance;

    public TowerController selectedTowerBtn;

    private SpriteRenderer spriteRenderer;

    private List<Tower> towers = new List<Tower>();

    private List<Collider2D> buildSites = new List<Collider2D>();

    private Collider2D buildSite;

    public static TowerManager getInstance()
    {
        if (instance == null)
        {
            instance = new TowerManager();
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
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        buildSite = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && selectedTowerBtn != null)
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
            if (hit.collider.tag == "buildSite")
            {
                buildSite = hit.collider;
                buildSite.tag = "buildSiteFull";
                registerBuilSite(buildSite);
                placeTower(hit);
            }
        }

        if(spriteRenderer.enabled)
        {
            followMouse();
        }
    }

    public void registerBuilSite(Collider2D buildSite)
    {
        buildSites.Add(buildSite);
    }

    public void registerTower(Tower tower)
    {
        towers.Add(tower);
    }

    public void renameBuildSiteTag()
    {
        foreach(Collider2D collider in buildSites)
        {
            collider.tag = "buildSite";
        }
        buildSites.Clear();
    }

    public void destroyAllTowers()
    {
        foreach (Tower tower in towers)
        {
            Destroy(tower.gameObject);
        }
        towers.Clear();
    }

    public void placeTower(RaycastHit2D hit)
    {
        if (!EventSystem.current.IsPointerOverGameObject() && selectedTowerBtn != null)
        {
            Tower newTower = Instantiate(selectedTowerBtn.TowerObject);
            newTower.transform.position = hit.transform.position;
            newTower.Site = hit.collider;
            registerTower(newTower);
            disableDragSprite();
        }
        selectedTowerBtn = null;
    }

    public void buyTower(int price)
    {
        GameManager.getInstance().subtractMoney(price);
    }

    public void selectedTower(TowerController tower)
    {
        selectedTowerBtn = tower;
        if (selectedTowerBtn.TowerPrice <= GameManager.getInstance().TotalMoney)
        {
            buyTower(selectedTowerBtn.TowerPrice);
            enableDragSprite(selectedTowerBtn.DragSprite);
        }
        
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
}
