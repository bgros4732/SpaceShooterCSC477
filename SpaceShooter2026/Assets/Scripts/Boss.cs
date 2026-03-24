using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
  public float health = 25f;
  public float attackInterval = 7f;
  public GameObject enemyBulletPrefab;
  public GameObject enemyPrefab;
  public Transform bulletSpawnPoint;
  public GameObject expoPrefab;

  private float attackTimer;
  private Transform player;
  private Slider bossHealthBar;

  void Start()
  {
    attackTimer = attackInterval;
    player = GameObject.FindWithTag("Player").transform;
    bossHealthBar = GetComponentInChildren<Slider>();
    bossHealthBar.maxValue = health;
    bossHealthBar.value = health;
  }

  void Update()
  {
    attackTimer -= Time.deltaTime;
    if (attackTimer <= 0f)
    {
      // Boss attacks randomly assigned with 50/50 chance
      if (Random.value > 0.5f)
      {
        AttackShoot();
      }
      else
      {
        AttackSpawnEnemies();
      }
      attackTimer = attackInterval;
    }
  }

  // Shoots 3 enemy projectiles at player's position
  private void AttackShoot()
  {
    StartCoroutine(ShootCoroutine());
  }

  private IEnumerator ShootCoroutine()
  {
    float[] yOffsets = { 1.7f, 0f, -1.7f };
    for (int i = 0; i < 3; i++)
    {
      Vector3 spawnPos = new Vector3(bulletSpawnPoint.position.x, bulletSpawnPoint.position.y + yOffsets[i], 0);
      Instantiate(enemyBulletPrefab, spawnPos, Quaternion.identity);
      yield return new WaitForSeconds(0.5f);
    }
  }

  // Spawns 3 updown enemies in 1 second intervals
  private void AttackSpawnEnemies()
  {
    StartCoroutine(SpawnBossMinions());
  }
  
  private IEnumerator SpawnBossMinions()
  {
    for (int i = 0; i < 3; i++)
    {
      Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y + (i * 1.5f), 0);
      GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
      enemy.GetComponent<Enemy>().enemyType = Enemy.EnemyType.Updown;
      yield return new WaitForSeconds(1f);
    }
  }

  private void OnCollisionEnter2D(Collision2D c)
  {
    if (c.gameObject.CompareTag("Bullet"))
    {
      Destroy(c.gameObject);
      health--;
      bossHealthBar.value = health;
      if (health <= 0)
      {
        var expoObj = Instantiate(expoPrefab, transform.position, Quaternion.identity);
        Destroy(expoObj, expoObj.GetComponent<ParticleSystem>().main.duration);
        Destroy(gameObject);
        Game.Instance.BossDefeated();
      }
    }
  }
}