using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject CreditsMenu;
    public GameObject OverlayCanvas;
    public GameObject WinScreen;

    public TMP_Text dificultyText;

    [SerializeField] GameObject runeMainMenu;

    public TMP_Text pointsText;
    public TMP_Text ComboText;
    public TMP_Text pointsWinScreenText;

    int maxCombo;

    void Start()
    {
        GameManager.OnGameStateChanged += GameStateHandler;
    }
    void Update()
    {
        getMaxCombo();
        if (GameManager.instance.State == GameState.Menu) { handleRotatingRune(); maxCombo = 0; }

        if (GameManager.instance.State == GameState.WinScreen) { pointsWinScreenText.SetText("You've got {0} Points\n in {1} moves \n with {2} max combo", GameManager.instance.getPoints(), GameManager.instance.getMoves(), maxCombo); }

        if (GameManager.instance.State == GameState.Play) {
            
            pointsText.SetText("Points: {0}", GameManager.instance.getPoints());
            if (GameManager.instance.getComboPoints() != 0)
            {
                ComboText.gameObject.SetActive(true);
                ComboText.SetText("Combo x{0}", GameManager.instance.getComboPoints());
            }
            else
            {
                ComboText.gameObject.SetActive(false);
            }
        }
    }
    void getMaxCombo()
    {
        if(GameManager.instance.getComboPoints() > maxCombo){ maxCombo = GameManager.instance.getComboPoints(); }
    }
    private void handleRotatingRune()
    {
       runeMainMenu.transform.Rotate(Vector3.up, -90.0f * Time.deltaTime);
        if ((int)runeMainMenu.transform.rotation.eulerAngles.y == 90 || (int)runeMainMenu.transform.rotation.eulerAngles.y == 270)
        {
            runeMainMenu.GetComponent<Image>().sprite = GameManager.instance.runeSpriteArray[Random.Range(1, 35)];
        }
    }
    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameStateHandler;
    }
    private void GameStateHandler(GameState state)
    {
        MainMenu.SetActive(state == GameState.Menu);
        CreditsMenu.SetActive(state == GameState.Credits);
    }
    public void goToMainMenu()
    {
        if (GameManager.instance.getCheckingRunes()==false)
        {
            GameManager.instance.UpdateGameState(GameState.Menu);
        }
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
                GameManager.instance.setDificulty(Dificulty.Easy);
                break;
            case 1:
                GameManager.instance.setDificulty(Dificulty.Hard);
                break;
        }

        switch (GameManager.instance.getDificulty())
        {
            case Dificulty.Easy:
                dificultyText.text = "Easy";
                break;
            case Dificulty.Hard:
                dificultyText.text = "Hard";
                break;
        }
       
    }
    public void startGame()
    {
        GameManager.instance.UpdateGameState(GameState.Play);
    }
    public void quitGame()
    {
        Application.Quit();
    }
}
