using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UI : MonoBehaviour {
  public GameObject uiTitle;
  public GameObject uiGameover;
  public GameObject uiLevel1Screen;
  public GameObject uiLevel2Screen;
  public GameObject uiLevel3Screen;
  public GameObject uiWinScreen;
  public GameObject bossHealthBar;
  public TextMeshProUGUI survivalTimerText;
  public GameObject timerBackground;

  public bool IsReady { get; private set; }

  private void Start() {
    uiGameover.SetActive(false);
    uiTitle.SetActive(true);
    uiLevel1Screen.SetActive(false);
    uiLevel2Screen.SetActive(false);
    uiLevel3Screen.SetActive(false);
    uiWinScreen.SetActive(false);
    bossHealthBar.SetActive(false);
    survivalTimerText.gameObject.SetActive(false);
    timerBackground.SetActive(false);
    SpaceShooterInput.Instance.DisableInput();
    IsReady = false;
  }

  void Update()
  {
    if (survivalTimerText != null)
    {
      survivalTimerText.text = "Time: " + Mathf.FloorToInt(Game.Instance.survivalTimer) + "s";
    }
  }

  // Starts survival mode
  public void StartSurvivalMode()
  {
    uiTitle.SetActive(false);
    Game.Instance.isSurvivalMode = true;
    survivalTimerText.gameObject.SetActive(true);
    timerBackground.SetActive(true);
    SpaceShooterInput.Instance.EnableInput();
    IsReady = true;
  }

  // Display level screen
  public void ShowLevelScreen(int level)
  {
    if (level == 1)
    {
      uiLevel1Screen.SetActive(true);
    }
    else if (level == 2)
    {
      uiLevel2Screen.SetActive(true);
    }
    else if (level == 3)
    {
      uiLevel3Screen.SetActive(true);
    }
    survivalTimerText.gameObject.SetActive(false);
    timerBackground.SetActive(false);
    SpaceShooterInput.Instance.DisableInput();
    IsReady = false;
  }

  public void ShowWinScreen()
  {
    uiWinScreen.SetActive(true);
    bossHealthBar.SetActive(false);
    survivalTimerText.gameObject.SetActive(false);
    timerBackground.SetActive(false);
    SpaceShooterInput.Instance.DisableInput();
    IsReady = false;
  }

  // User presses button to start level
  public void StartLevel()
  {
    uiLevel1Screen.SetActive(false);
    uiLevel2Screen.SetActive(false);
    uiLevel3Screen.SetActive(false);
    survivalTimerText.gameObject.SetActive(true);
    timerBackground.SetActive(true);
    SpaceShooterInput.Instance.EnableInput();
    Game.Instance.CheckSpawnBoss();
    IsReady = true;
  }
  
  public void ShowGameOver() {
    uiGameover.SetActive(true);
    bossHealthBar.SetActive(false);
    survivalTimerText.gameObject.SetActive(false);
    timerBackground.SetActive(false);
    SpaceShooterInput.Instance.DisableInput();
    IsReady = false;
  }

  public void RestartGame() {
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }

  public void StartGame() {
    uiTitle.SetActive(false);
    ShowLevelScreen(1);
  }
}
