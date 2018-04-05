using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SpellBook : MonoBehaviour {
    public static UI_SpellBook Instance { get; set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    private Text subtitleText, passiveText, meleeText, rangedText, glyphText;
    private string subtitle,passiveDescription, meleeDescription, rangedDescription, glyphDescription; 

	// Use this for initialization
	void Start () {
        subtitleText = transform.FindChild("Subtitle").GetComponent<Text>();
        passiveText = transform.FindChild("Passive").FindChild("Description").GetComponent<Text>();
        meleeText = transform.FindChild("Melee").FindChild("Description").GetComponent<Text>();
        rangedText = transform.FindChild("Ranged").FindChild("Description").GetComponent<Text>();
        glyphText = transform.FindChild("Glyph").FindChild("Description").GetComponent<Text>();
        setNeutral();
    }

    public void updateDisplay()
    {
        subtitleText.text = subtitle;
        passiveText.text = passiveDescription;
        meleeText.text = meleeDescription;
        rangedText.text = rangedDescription;
        glyphText.text = glyphDescription;
    }

    public void setNeutral()
    {
        subtitle = "Neutral";
        passiveDescription = "After casting a neutral spell you can change it's element by switching to a different element.";
        meleeDescription = "Use your wand to attack nearby enemies, press the attack button repeatedly to chain attacks.";
        rangedDescription = "Cast a neutral spell in the direction you're facing.";
        glyphDescription = "When activated a bomb apears from the glyph and explodes.";
        updateDisplay();
    }

    public void setFire()
    {
        subtitle = "Fire";
        passiveDescription = "When this element is active your wand will ignite and iluminate your suroundings.";
        meleeDescription = "Your attacks are now enhanced with fire damage.";
        rangedDescription = "Cast a cone of fire in a short distance in front of you.";
        glyphDescription = "When activated a fire will apear at the location of this glyph";
        updateDisplay();
    }
    public void setIce()
    {
        subtitle = "Ice";
        passiveDescription = "When active, this element allows you to freeze water so that you can walk on top of it.";
        meleeDescription = "Your attacks are now enhanced with Ice damage.";
        rangedDescription = "Cast a cone of Ice cold magic in front of you.";
        glyphDescription = "When activated this glyph will create a block of Ice at it's position";
        updateDisplay();
    }
    public void setWind()
    {
        subtitle = "Wind";
        passiveDescription = "When active, this element allows you to perform your dash ability in the air.";
        meleeDescription = "Your attacks can now repel weak enemy projectiles.";
        rangedDescription = "Cast a gust of wind in the direction you're facing. This can push enemies and certain objects.";
        glyphDescription = "When activated this glyph will create a continuous gust of wind at it's location.";
        updateDisplay();
    }
    public void setElectric()
    {
        subtitle = "Electric";
        passiveDescription = "When active, this element will change your dash ability into a blink ability. Use it to teleport short distances.";
        meleeDescription = "(Unimplemented)";
        rangedDescription = "(Unimplemented)";
        glyphDescription = "(Unimplemented)";
        updateDisplay();
    }


}
