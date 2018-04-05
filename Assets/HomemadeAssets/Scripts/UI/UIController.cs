using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

//Last clean: 31/01/2018

public class UIController : MonoBehaviour {
    public static UIController Instance { get; set; }

    //HUD text
    [SerializeField]
    private Text action;
    [SerializeField]
    private Text interaction;

    //Debuging display text
    [SerializeField]
    private Text state, spellType;

    //PauseScreen
    [SerializeField]
    private GameObject pauseScreen;

    private GameObject inventoryScreen;
    private GameObject spellbookscreen;
    private GameObject relationshipScreen;

    private bool paused;

    [SerializeField]
    public GameObject MenuScreen;

    //TextDisplay
    private TextController textDisplay;
    private GameObject textBox;

    [SerializeField]
    private GameObject indicator;
    [SerializeField]
    private Text healthString, manaString;

    private Sprite voidSprite, fireSprite, iceSprite, windSprite, electricSprite;
    private RectTransform healthBar;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start () {
        inventoryScreen = UI_Inventory.Instance.gameObject;
        inventoryScreen.SetActive(false);
        spellbookscreen = UI_SpellBook.Instance.gameObject;
        spellbookscreen.SetActive(false);
        relationshipScreen = UI_Relationship.Instance.gameObject;
        relationshipScreen.SetActive(false);

        healthBar = transform.FindChildByRecursive("HUD_HP_Foreground").GetComponent<RectTransform>();

        loadSprites();

        pauseScreen.SetActive(false);
        paused = false;
    }
    void Update()
    {
        if (Input.GetButtonDown("B"))
        {
            //closeSubscreen();
        }
        if (Input.GetButtonDown("Start"))
        {
            if (paused)
            {
                resume();
            }
            else if (!MenuScreen.activeSelf)
            {
                pause();
            }
        }
    }

    //get methods:
    public GameObject getInteractIcon()
    {
        return indicator;
    }

    public string getActionButton()
    {
        return action.text;
    }

    //Set methods:

    public void setActionButton(string actiontxt)
    {
        action.text = actiontxt;
    }

    public void setInteractButton(string interactiontxt)
    {
        interaction.text = interactiontxt;
    }

    public void setPlayerState(string statetxt)
    {
        state.text = "State: " + statetxt;
    }

    public void setSpellState(string statetxt)
    {
        spellType.text = "Spell: " + statetxt;
    }

    //Update methods

    public void updateHealth(Stat health)
    {
        healthString.text = health.statValue + "/" + health.statMax;
        Debug.Log(health.statValue + " " + health.statMax + " " + healthBar.transform.parent.GetComponent<RectTransform>().rect.size.x);
        float size = (float)health.statValue / (float)health.statMax * healthBar.transform.parent.GetComponent<RectTransform>().rect.size.x;
        Debug.Log(size);
        healthBar.sizeDelta = new Vector2(size, healthBar.sizeDelta.y);
    }

    public void updateMana(Stat mana)
    {
        manaString.text = mana.statValue + "/" + mana.statMax;
    }

    //Various methods
    public void SaveGame()
    {
        GameController.Instance.SaveGame();
    }
    

    public void pause()
    {
        GameController.Instance.pause();
        pauseScreen.SetActive(true);
        paused = true;
        FollowCamera.Instance.activated = false;
    }
    public void resume()
    {
        paused = false;
        GameController.Instance.pause();
        closeSubscreen();
        pauseScreen.SetActive(false);
        FollowCamera.Instance.activated = true;
    }

  
    public void quitToMenu()
    {
        resume();
        GameController.Instance.quit();
        FollowCamera.Instance.activated = false;
    }

    public void openInventory()
    {
        pauseScreen.SetActive(false);
        inventoryScreen.SetActive(true);
        inventoryScreen.GetComponent<UI_Inventory>().open();
    }

    public void openRelationships()
    {
        pauseScreen.SetActive(false);
        relationshipScreen.SetActive(true);
    }
    public void openSpellBook()
    {
        pauseScreen.SetActive(false);
        spellbookscreen.SetActive(true);
    }


    private void loadSprites()
    {
        voidSprite = loadImage("VoidElement");
        fireSprite = loadImage("FireElement");
        iceSprite = loadImage("IceElement");
        windSprite = loadImage("WindElement");
        electricSprite = loadImage("ElectricElement");
    }
    private Sprite loadImage(string name)
    {
        try
        {
            string path = ".\\Assets\\HomemadeAssets\\Sprites\\" + name + ".png";
            byte[] fileData = File.ReadAllBytes(path);
            var tex = new Texture2D(110, 110);
            tex.LoadImage(fileData);
            return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
        }
        catch (IOException e)
        {
            Debug.Log("Exception: " + e.Message);
        }
        return null;
    }


    public void setActiveElement(SpellType element)
    {
        Transform hud_void = transform.FindChild("HUD").FindChild("HUD_Element").FindChild("Void");
        Transform hud_fire = transform.FindChild("HUD").FindChild("HUD_Element").FindChild("Fire");
        Transform hud_ice = transform.FindChild("HUD").FindChild("HUD_Element").FindChild("Ice");
        Transform hud_wind = transform.FindChild("HUD").FindChild("HUD_Element").FindChild("Wind");
        Transform hud_electric = transform.FindChild("HUD").FindChild("HUD_Element").FindChild("Electric");

        hud_void.GetComponent<Image>().sprite = voidSprite;
        hud_fire.GetComponent<Image>().sprite = fireSprite;
        hud_ice.GetComponent<Image>().sprite = iceSprite;
        hud_wind.GetComponent<Image>().sprite = windSprite;
        hud_electric.GetComponent<Image>().sprite = electricSprite;


        hud_fire.gameObject.SetActive(false);
        hud_ice.gameObject.SetActive(false);
        hud_wind.gameObject.SetActive(false);
        hud_electric.gameObject.SetActive(false);

        if (PlayerController.Instance.unlocked.abilities[1])
        {
            hud_fire.gameObject.SetActive(true);
        }
        if (PlayerController.Instance.unlocked.abilities[2])
        {
            hud_ice.gameObject.SetActive(true);
        }
        if (PlayerController.Instance.unlocked.abilities[3])
        {
            hud_wind.gameObject.SetActive(true);
        }
        if (PlayerController.Instance.unlocked.abilities[4])
        {
            hud_electric.gameObject.SetActive(true);
        }

        if (element == SpellType.Fire)
        {
            hud_void.GetComponent<Image>().sprite = fireSprite;
            hud_fire.GetComponent<Image>().sprite = voidSprite;
        }
        else if (element == SpellType.Ice)
        {
            hud_void.GetComponent<Image>().sprite = iceSprite;
            hud_ice.GetComponent<Image>().sprite = voidSprite;
        }
        else if (element == SpellType.Wind)
        {
            hud_void.GetComponent<Image>().sprite = windSprite;
            hud_wind.GetComponent<Image>().sprite = voidSprite;
        }
        else if (element == SpellType.Electric)
        {
            hud_void.GetComponent<Image>().sprite = electricSprite;
            hud_electric.GetComponent<Image>().sprite = voidSprite;
        }
    }

    public void toggleCrosshair(bool active)
    {
        transform.FindChild("Crosshair").gameObject.SetActive(active);
    }

    public void closeSubscreen()
    {
        pauseScreen.SetActive(true);
        inventoryScreen.SetActive(false);
        relationshipScreen.SetActive(false);
        spellbookscreen.SetActive(false);
    }

}
