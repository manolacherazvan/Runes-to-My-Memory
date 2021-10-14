using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject DificultyMenu;
    public GameObject CreditsMenu;
    public GameObject OverlayCanvas;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.OnGameStateChanged += GameStateHandler;
    }
    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameStateHandler;
    }
    private void GameStateHandler(GameState state)
    {
        MainMenu.SetActive(state == GameState.Menu);
        DificultyMenu.SetActive(state == GameState.DifficultySelect);
        CreditsMenu.SetActive(state == GameState.Credits);
    }
    public void goToMainMenu()
    {
        GameManager.instance.UpdateGameState(GameState.Menu);
    }
    public void goToDificultyMenu()
    {
        GameManager.instance.UpdateGameState(GameState.DifficultySelect);
    }
    public void goToCreditsScreen()
    {
        GameManager.instance.UpdateGameState(GameState.Credits);
    }

  

    public void selectDificuty(int i)
    {
        switch (i)
        {
            case 0:
                GameManager.instance.setDificulty(Dificulty.Normal);
                break;
            case 1:
                GameManager.instance.setDificulty(Dificulty.Hard);
                break;

        }
        GameManager.instance.UpdateGameState(GameState.Play);
    }
    public void quitGame()
    {
        Application.Quit();
    }
}
