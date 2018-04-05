using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour {

	public void NewGame()
    {
        GameController.Instance.newGame();
    }

    public void LoadGame()
    {
        GameController.Instance.loadLevel();
    }

    public void QuitGame()
    {
        GameController.Instance.quitGame();
    }
}
