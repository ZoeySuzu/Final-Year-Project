using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveHandler {


    private Save PrepareSaveFile()
    {
        Save save = new Save();

        List<CharacterController> characters = GameController.Instance.characters;
        foreach(CharacterController cc in characters)
        {
            save.relationshipValues.Add(cc.getFP());
        }
        save.LevelList = GameController.Instance.levelSystem.levels;
        save.PlayerStats = PlayerController.Instance.stats;
        save.inventory = GameController.Instance.inventory;
        save.unlocks = PlayerController.Instance.unlocked;

        return save;
    }


    private void UnpackSaveFile(Save save)
    {
        Debug.Log("Unpacking");
        PlayerController.Instance.updateStats(save.PlayerStats);
        PlayerController.Instance.unlocked = save.unlocks;
        GameController.Instance.inventory = save.inventory;
        GameController.Instance.levelSystem.loadLevelList(save.LevelList);
    }

    public void SaveGame()
    {
        Debug.Log("Saving Game");
        Save save = PrepareSaveFile();
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/TestSave.zsf");
        bf.Serialize(file, save);
        file.Close();
        Debug.Log("Game saved");

    }

    public void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + "/TestSave.zsf"))
        {
            Debug.Log("Loading save");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/TestSave.zsf", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();
            UnpackSaveFile(save);
            Debug.Log("Game loaded");
        }       
    }
}
