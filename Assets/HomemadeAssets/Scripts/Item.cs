using System.IO;
using UnityEngine;

[System.Serializable]
public abstract class Item {

    public string name { get; protected set; }
    public string description { get; protected set; }

    public ItemType iType;


    protected void loadDescription()
    {
        try
        {
            string path = ".\\Assets\\HomemadeAssets\\ItemDescription\\" + name + ".txt";
            StreamReader sr = new StreamReader(path);
            description = sr.ReadToEnd();
        }
        catch (IOException e)
        {
            Debug.Log("Exception: " + e.Message);
        }
    }

    public Sprite loadImage()
    {
        try
        {
            string path = ".\\Assets\\HomemadeAssets\\ItemImages\\" + name + ".png";
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
}
