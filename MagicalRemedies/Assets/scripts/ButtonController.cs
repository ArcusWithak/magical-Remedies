using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    GameManager manager;

    public GameObject Buttons;
    public GameObject InstructionMessage;

    bool move;
    Vector3 position;

    private void Start()
    {
        manager = GameManager.instance;
    }


    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadStartScreen()
    {
        manager.LoadLevel(0);
    }

    public void PlayCutScene()
    {
        manager.LoadLevel(manager.NumberOfMenuLevels - 1);
    }

    public void LoadFirstLevel()
    {
        manager.LoadLevel(manager.NumberOfMenuLevels);
    }

    public void Retry()
    {
        if (manager.currentLevel <= 1)
        {
            manager.LoadLevel(manager.NumberOfMenuLevels);
        }
        else if (manager.currentLevel < 5)
        {
            manager.LoadLevel(manager.NumberOfMenuLevels + 1);
        }
        else
        {
            manager.LoadLevel(manager.NumberOfMenuLevels + 2);
        }
    }

    public void HowToPlay()
    {
        Buttons.SetActive(false);
        InstructionMessage.SetActive(true);
    }

    public void Return()
    {
        Buttons.SetActive(true);
        InstructionMessage.SetActive(false);
    }

}
