using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour {
  public GameObject uiTitle;
  public GameObject uiGameover;
  public GameObject uiLevel1Screen;
  public GameObject uiLevel2Screen;
  public GameObject uiLevel3Screen;
  public GameObject uiWinScreen;

  public bool IsReady { get; private set; }

  private void Start() {
    uiGameover.SetActive(false);
    uiTitle.SetActive(true);
    uiLevel1Screen.SetActive(false);
    uiLevel2Screen.SetActive(false);
    uiLevel3Screen.SetActive(false);
    uiWinScreen.SetActive(false);
    SpaceShooterInput.Instance.DisableInput();
    IsReady = false;
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
    SpaceShooterInput.Instance.DisableInput();
    IsReady = false;
  }

  public void ShowWinScreen()
  {
    uiWinScreen.SetActive(true);
    SpaceShooterInput.Instance.DisableInput();
    IsReady = false;
  }

  // User presses button to start level
  public void StartLevel()
  {
    uiLevel1Screen.SetActive(false);
    uiLevel2Screen.SetActive(false);
    uiLevel3Screen.SetActive(false);
    SpaceShooterInput.Instance.EnableInput();
    Game.Instance.CheckSpawnBoss();
    IsReady = true;
  }
  
  public void ShowGameOver() {
    uiGameover.SetActive(true);
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
