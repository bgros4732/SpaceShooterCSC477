using UnityEngine;

public class Enemy : MonoBehaviour {
  // set in inspector
  public float speed;
  public GameObject expoPrefab;

  // enemy movement types
  public enum EnemyType { Straight, Updown, Shooter}
  public EnemyType enemyType;

  // Movement limits for Updown enemy
  public const float Y_LIMIT = 4.6f;
  public float verticalDirection = 1f;

  // Fields for Shooter enemy
  public GameObject enemyBulletPrefab;
  public float fireRate = 1f;
  private float fireTimer;

  public Transform bulletSpawnPoint;

  void Update()
  {
    if (enemyType == EnemyType.Straight)
    {
      transform.Translate(Vector3.left * speed * Time.deltaTime);
    }
    else if (enemyType == EnemyType.Updown)
    {
      transform.Translate(Vector3.left * speed * Time.deltaTime);
      if (transform.position.y >= Y_LIMIT)
      {
        verticalDirection = -1f;
      }
      else if (transform.position.y <= -Y_LIMIT)
      {
        verticalDirection = 1f;
      }
      transform.Translate(Vector3.up * verticalDirection * speed * Time.deltaTime * 1.5f);
    }
    else if (enemyType == EnemyType.Shooter)
    {
      transform.Translate(Vector3.left * speed * Time.deltaTime);
      fireTimer -= Time.deltaTime;
      if (fireTimer <= 0f)
      {
        Instantiate(enemyBulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        fireTimer = fireRate;
      }
    }
  }

  private void OnCollisionEnter2D(Collision2D c) {
    if (c.gameObject.CompareTag("Bullet")) {
      var expoObj = Instantiate(expoPrefab, transform.position, Quaternion.identity);
      Destroy(expoObj, expoObj.GetComponent<ParticleSystem>().main.duration);
      Destroy(gameObject);
      Destroy(c.gameObject);
      Score.Instance.HitEnemy();
      Game.Instance.EnemyKilled();
    }
    else if (c.gameObject.CompareTag("Player")) {
      Destroy(gameObject);
      c.gameObject.GetComponent<Player>().DamageFromEnemy();
    }
  }
}
