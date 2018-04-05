using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelHandler {

    public Level activeLevel;
    public List<Level> levels;

    public void initLevelList()
    {
        levels = new List<Level>();
        levels.Add(new Level("DebugLevel"));
        levels.Add(new Level("SchoolTutorial"));
        levels.Add(new Level("windArea"));
        levels.Add(new Level("fireArea"));
    }

    public void loadLevelList(List<Level> _levels)
    {
        levels = _levels;
    }

    public void unloadAll()
    {
        activeLevel.unloadLevel();
        activeLevel = null;
    }

    public void loadLevel(Level level)
    {
        if (activeLevel != null) {
            activeLevel.unloadLevel();
        }
        activeLevel = level;
        activeLevel.loadLevel();
    }

}
