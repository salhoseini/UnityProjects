using UnityEngine.EventSystems;
using UnityEngine;

public class BuildSiteManager : MonoBehaviour {

    public static BuildSiteManager instance;

    [SerializeField] public BuildSiteBtn selectedBuildSiteBtn;

    private SpriteRenderer spriteRenderer;
    private Collider2D buildSite;

    public static BuildSiteManager getInstance()
    {
        if (instance == null)
        {
            instance = new BuildSiteManager();
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
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
            if (hit.collider.tag == "groundBuildSite")
            {
                buildSite = hit.collider;
                buildSite.tag = "buildSite";
                placeBuildSite(hit);
            }
        }

        if (spriteRenderer.enabled)
        {
            followMouse();
        }
    }

    public void followMouse()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(transform.position.x, transform.position.y);
    }

    public void placeBuildSite(RaycastHit2D hit)
    {
        if (!EventSystem.current.IsPointerOverGameObject() && selectedBuildSiteBtn != null)
        {
            BuildStie newBuildSite = Instantiate(selectedBuildSiteBtn.BuildSite);
            newBuildSite.transform.position = hit.transform.position;
            newBuildSite.Site = hit.collider;
            //registerTower(newTower);
            disableDragSprite();
        }
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

    public void selectBuildSite(BuildSiteBtn buildSite)
    {
        selectedBuildSiteBtn = buildSite;
        if (selectedBuildSiteBtn.BuildSitePrice <= GameManager.getInstance().TotalMoney)
        {
            buyBuildSite(selectedBuildSiteBtn.BuildSitePrice);
            enableDragSprite(selectedBuildSiteBtn.DragSprite);
        }

    }

    public void buyBuildSite(int price)
    {
        GameManager.getInstance().subtractMoney(price);
    }
}
