using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour
{
  public float health = 10f;
  public float attackInterval = 7f;
  public GameObject enemyBulletPrefab;
  public GameObject enemyPrefab;
  public Transform bulletSpawnPoint;
  public GameObject expoPrefab;
  public UI ui;

  private float attackTimer;
  private Transform player;

  void Start()
  {
    attackTimer = attackInterval;
    player = GameObject.FindWithTag("Player").transform;
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
    Vector3 direction = (player.position - bulletSpawnPoint.position).normalized;
    for (int i = 0; i < 3; i++)
    {
      GameObject bullet = Instantiate(enemyBulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
      bullet.GetComponent<Rigidbody2D>().linearVelocity = direction * 10f;
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