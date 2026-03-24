using UnityEngine;

public class Game : MonoBehaviour {
  // set in inspector
  public float enemySpawnDelay;
  public GameObject enemyPrefab;
  public GameObject powerupPrefab;
  public BoxCollider2D spawnRange;
  public UI ui;
  public GameObject playerObject;

  // private fields
  private float powerUpDelay;
  private float enemySpawnTimer;
  private float powerupSpawnTimer;
  private int currentLevel = 1;
  private int enemyKillCount = 0;

  public static Game Instance { get; private set; }

  private void Awake()
  {
    Instance = this;
  }

  private void Start() {
    powerUpDelay = Random.Range(5f, 10f);
    powerupSpawnTimer = 0;
  }

  private void SpawnEnemy() {
    Vector3 enemySpawnPt = new Vector3(
        Random.Range(spawnRange.bounds.min.x, spawnRange.bounds.max.x),
        Random.Range(spawnRange.bounds.min.y, spawnRange.bounds.max.y),
        0);
    // Spawns certain types of enemies randomly for each level
    GameObject enemy = Instantiate(enemyPrefab, enemySpawnPt, Quaternion.identity);
    Enemy enemyScript = enemy.GetComponent<Enemy>();

    if (currentLevel == 1)
    {
      enemyScript.enemyType = Random.value > 0.5f ?
        Enemy.EnemyType.Straight :
        Enemy.EnemyType.Updown;
    }
    else if (currentLevel == 2)
    {
      float randomVal = Random.value;
      if (randomVal < 0.15f)
      {
        enemyScript.enemyType = Enemy.EnemyType.Straight;
      }
      else if (randomVal < 0.30f)
      {
        enemyScript.enemyType = Enemy.EnemyType.Updown;
      }
      else
      {
        enemyScript.enemyType = Enemy.EnemyType.Shooter;
      }
    }
    else if (currentLevel == 3)
    {
      return;
    }
    
  }

  // Enemy kill tracking
  public void EnemyKilled()
  {
    enemyKillCount++;
    if (currentLevel == 1 && enemyKillCount >= 15)
    {
      LoadLevel(2);
    }
    else if (currentLevel == 2 && enemyKillCount >= 15)
    {
      LoadLevel(3);
    }
  }

  // Level loader that resets enemy kill count and player health each level
  private void LoadLevel(int level)
  {
    currentLevel = level;
    enemyKillCount = 0;
    playerObject.GetComponent<Player>().ResetHealth();
    foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
    {
      Destroy(enemy);
    }
    ui.ShowLevelScreen(level);
  }

  private void SpawnPowerup() {
    Vector3 powerupSpawnPt = new Vector3(
        Random.Range(spawnRange.bounds.min.x, spawnRange.bounds.max.x),
        Random.Range(spawnRange.bounds.min.y, spawnRange.bounds.max.y),
        0);
    Instantiate(powerupPrefab, powerupSpawnPt, Quaternion.identity);
  }
  void Update() {
    if (!ui.IsReady)
    {
      return;
    }

    // Stop spawning enemies on level 3 and check spawn timer
    if (currentLevel < 3)
    {
      enemySpawnTimer += Time.deltaTime;
      if (enemySpawnTimer >= enemySpawnDelay)
      {
        SpawnEnemy();
        enemySpawnTimer = 0.0f;
      }
    }

    // check spawn powerup
    powerupSpawnTimer += Time.deltaTime;
    if (powerupSpawnTimer >= powerUpDelay) {
      SpawnPowerup();
      powerUpDelay = Random.Range(5, 10);
      powerupSpawnTimer = 0.0f;
    }
  }
}
