using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameState State;

    public static event Action<GameState> OnGameStateChanged;

    private Dificulty selectedDificulty;

    public GameObject GridHolder;
    public MenuManager MenuManager;

    public Sprite[] runeSpriteArray;

    public GameObject[,] Runes;

    public int[,] runeIndexes;

    GameObject firstSelectedRune;
    GameObject secondSelectedRune;

    Boolean checkingRunes=false;

    private int numberOfRunes=0;

    private int points;
    private int comboPoints;
    private int moves;

    Boolean debbugingMode=false;

    public Camera maincamera;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UpdateGameState(GameState.Menu);
    }
    public void UpdateGameState(GameState newState)
    {
        State = newState;
        switch (newState) {
            case GameState.Menu:
                HandleMenuState();
                break;
            case GameState.Play:
                handlePlayState();
                break;
            case GameState.WinScreen:
                handleWinScreenState();
                break;
            case GameState.LoseScreen:
                break;
            case GameState.Credits:
                break;
        }
        OnGameStateChanged?.Invoke(newState);
    }

    private void HandleMenuState()
    {
        GridHolder.SetActive(State == GameState.Play);
        MenuManager.OverlayCanvas.SetActive(State == GameState.Play);
        clearGame();
    }
    private void clearGame()
    {
        maincamera.backgroundColor = new Color(0.09f, 0.1f, 0.1f);
        GameObject[] cachedRunes = GameObject.FindGameObjectsWithTag("Rune");
        foreach (GameObject cachedRune in cachedRunes)
            GameObject.Destroy(cachedRune);
        GameObject[] cachedParticles = GameObject.FindGameObjectsWithTag("Particles");
        foreach (GameObject cachedParticle in cachedParticles)
            GameObject.Destroy(cachedParticle);
        runeIndexes = null;
        Runes = null;
        points = 0;
        comboPoints = 0;
        moves = 0;
    }
    private void handlePlayState()
    {
        GridHolder.SetActive(State == GameState.Play);
        MenuManager.OverlayCanvas.SetActive(State == GameState.Play);
        switch (selectedDificulty)
        {
            case Dificulty.Easy:
                generateRuneIndex(4, 4);
                GridHolder.GetComponent<GridManager>().generateGrid(4, 4);
                break;
            case Dificulty.Hard:
                generateRuneIndex(4, 8);
                GridHolder.GetComponent<GridManager>().generateGrid(4, 8);
                break;
        }
    }
    private void generateRuneIndex(int randuri, int coloane)
    {
        int[] eligibleRunes;
        numberOfRunes = randuri * coloane;
        eligibleRunes = new int[numberOfRunes];
        for (int i = 0; i < eligibleRunes.Length/2; i++)
        {
            eligibleRunes[i] = UnityEngine.Random.Range(1, 35);
            eligibleRunes[i + eligibleRunes.Length / 2] = eligibleRunes[i];
        }
        shuffleEligibleRunes(eligibleRunes);

        runeIndexes = new int[randuri, coloane];
        Runes = new GameObject[randuri, coloane];
        int indexEligibleRune = 0;
        for (int i = 0; i < randuri; i++)
        {
            for (int j = 0; j <coloane ; j++)
            {
                runeIndexes[i, j] = eligibleRunes[indexEligibleRune];
                indexEligibleRune++;
            }
        }
    }
    void shuffleEligibleRunes(int[] eligibleRunes)
    {
        for (int t = 0; t < eligibleRunes.Length; t++)
        {
            int tmp = eligibleRunes[t];
            int r = UnityEngine.Random.Range(t, eligibleRunes.Length);
            eligibleRunes[t] = eligibleRunes[r];
            eligibleRunes[r] = tmp;
        }
    }
    public void selectRune(GameObject rune)
    {
        if (!checkingRunes)    
        {
            moves++;
            AudioManager.instance.PlayRuneSound();
            if (firstSelectedRune == null)
            {
                firstSelectedRune = rune;
                firstSelectedRune.GetComponent<Rune>().showRune();
                firstSelectedRune.GetComponent<BoxCollider2D>().enabled = false;
            }
            else
            {
                secondSelectedRune = rune;
                secondSelectedRune.GetComponent<Rune>().showRune();
                StartCoroutine(checkRunes());
            }
        }
        else
        {
            Debug.Log("Wait");
        }
    }

    IEnumerator checkRunes()
    {
        Debug.Log("Started Checking runes");
        checkingRunes = true;
        maincamera.backgroundColor =new Color(0.09f,0.1f,0.1f);
        yield return new WaitForSeconds(1);

        if (firstSelectedRune.GetComponent<Rune>().getIndex() == secondSelectedRune.GetComponent<Rune>().getIndex())
        {
            AudioManager.instance.PlayRuneBoomSound();
            Debug.Log("Match: " + firstSelectedRune.GetComponent<Rune>().getIndex() + "=" + secondSelectedRune.GetComponent<Rune>().getIndex());
            Destroy(firstSelectedRune.gameObject);
            Destroy(secondSelectedRune.gameObject);
            numberOfRunes -= 2;
            comboPoints += 1;
            addPoints();
            maincamera.backgroundColor = new Color(0.09f, 0.1f, 0.15f);
        }
        else
        {
            comboPoints = 0;
            firstSelectedRune.GetComponent<BoxCollider2D>().enabled = true;
            firstSelectedRune.GetComponent<Rune>().coverRune();
            secondSelectedRune.GetComponent<Rune>().coverRune();
            maincamera.backgroundColor = new Color(0.15f, 0.1f, 0.1f);
        }
        firstSelectedRune = null;
        secondSelectedRune = null;
        checkingRunes = false;
        
        if (numberOfRunes == 0)
        {
            UpdateGameState(GameState.WinScreen);
        }
    }
    private void handleWinScreenState()
    {
        MenuManager.OverlayCanvas.SetActive(false);
        MenuManager.WinScreen.SetActive(true);
    }
    public void setDebugMode()
    {
        if (!debbugingMode)
        {
            debbugingMode = true;
        }
        else
        {
            debbugingMode = false;
        }
    }
    public bool getDebugMode()
    {
        return debbugingMode;
    }

    public void addPoints()
    {
        points += 10*comboPoints;
    }
    public int getPoints()
    {
        return points;
    }
    public int getComboPoints()
    {
        return comboPoints;
    }
    public int getMoves()
    {
        return moves;
    }
    public bool getCheckingRunes()
    {
        return this.checkingRunes;
    }
    public void setDificulty(Dificulty dificulty)
    {
        this.selectedDificulty = dificulty;
    }
    public Dificulty getDificulty()
    {
        return selectedDificulty;
    }

}
public enum GameState
{
    Menu,
    Play,
    WinScreen,
    LoseScreen,
    Credits
}
public enum Dificulty
{
    Easy,
    Hard
}