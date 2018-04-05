using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Level {

    public string name;
    bool visited;
    Dictionary<string, float[]> teleportLocations;

    public Level(string _name)
    {
        name = _name;
    }

    public void loadLevel()
    {
        SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
        if (!visited)
        {
            teleportLocations = new Dictionary<string, float[]>();
            visited = true;
        }
        else
        {
            float[] position;
            teleportLocations.TryGetValue("Main Entrance", out position);
            PlayerController.Instance.transform.position = new Vector3(position[0],position[1],position[2]);
        }
    }

    public void unloadLevel()
    {
        SceneManager.UnloadSceneAsync(name);
    }

    public void addTeleportLocation(string _name, float[] _location)
    {
        if (!teleportLocations.ContainsKey(_name))
        {
            Debug.Log("Added to teleport list " + _name);
            teleportLocations.Add(_name, _location);
        }
        else
        {
            Debug.Log("Teleporter allready in list");
        }
    }

    public Dictionary<string,float[]> getTeleportLocations()
    {
        return teleportLocations;
    }
}
