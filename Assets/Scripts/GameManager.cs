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

    public GameObject OverlayCanvas;

    public int[,] runeIndexes;
    public Sprite[] runeSpriteArray;
    public GameObject[,] Runes;

    GameObject firstSelectedRune;
    GameObject secondSelectedRune;
    Boolean checkingRunes=false;

    public int[] eligibleRunes;

    Boolean debbugingMode=false;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UpdateGameState(GameState.Menu);
    }


    void Update()
    {

    }
    public void UpdateGameState(GameState newState)
    {
        State = newState;
        switch (newState) {
            case GameState.Menu:
                handleMenuState();
                break;
            case GameState.DifficultySelect:
                handleDifficultySelectState();
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

    private void handleMenuState()
    {
        GridHolder.SetActive(State == GameState.Play);
        OverlayCanvas.SetActive(State == GameState.Play);

        GameObject[] cachedRunes = GameObject.FindGameObjectsWithTag("Rune");
        foreach (GameObject cachedRune in cachedRunes)
            GameObject.Destroy(cachedRune);
    }

    private void handleDifficultySelectState()
    {

    }
    public void setDificulty(Dificulty dificulty)
    {
        this.selectedDificulty = dificulty;
    }

    private void handlePlayState()
    {
        GridHolder.SetActive(State == GameState.Play);
        OverlayCanvas.SetActive(State == GameState.Play);
        switch (selectedDificulty)
        {
            case Dificulty.Normal:
                generateRuneIndex(4, 4);
                GridHolder.GetComponent<GridManager>().generateGrid(4, 4);
                break;
            case Dificulty.Hard:
                generateRuneIndex(8, 8);
                GridHolder.GetComponent<GridManager>().generateGrid(6, 8);
                break;
        }
    }

    private void generateRuneIndex(int randuri, int coloane)
    {
        int numarRune = randuri * coloane;
        eligibleRunes = new int[numarRune];

        for (int i = 0; i < eligibleRunes.Length/2; i++)
        {
            eligibleRunes[i] = UnityEngine.Random.Range(1, 35);
            eligibleRunes[i + eligibleRunes.Length / 2] = eligibleRunes[i];
        }
        shuffleEligibleRunes();
        runeIndexes = new int[randuri, coloane];
        Runes = new GameObject[randuri, coloane];
        int indexEligibleRune = 0;
        for (int i = 0; i < coloane; i++)
        {
            for (int j = 0; j < randuri; j++)
            {
                runeIndexes[i, j] = eligibleRunes[indexEligibleRune];
                indexEligibleRune++;
            }
        }
    }
    void shuffleEligibleRunes()
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
            if (firstSelectedRune == null)
            {
                firstSelectedRune = rune;
                firstSelectedRune.GetComponent<Rune>().showRune();
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

        yield return new WaitForSeconds(1);

        if (firstSelectedRune.GetComponent<Rune>().getIndex() == secondSelectedRune.GetComponent<Rune>().getIndex())
        {
            Debug.Log("it works: " + firstSelectedRune.GetComponent<Rune>().getIndex() + "=" + secondSelectedRune.GetComponent<Rune>().getIndex());
            Destroy(firstSelectedRune.gameObject);
            Destroy(secondSelectedRune.gameObject);
        }
        else
        {
            firstSelectedRune.GetComponent<Rune>().coverRune();
            secondSelectedRune.GetComponent<Rune>().coverRune();
        }
        firstSelectedRune = null;
        secondSelectedRune = null;
        checkingRunes = false;
        Debug.Log("Finished Checking runes");
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
    private void handleWinScreenState()
    {

    }
}

public enum GameState
{
    Menu,
    DifficultySelect,
    Play,
    WinScreen,
    LoseScreen,
    Credits
}
public enum Dificulty
{
    Normal,
    Hard
}