using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
  public float speed = 15f;

  private void Update()
  {
    this.transform.Translate(Vector3.left * speed * Time.deltaTime);
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.CompareTag("Player"))
    {
      collision.gameObject.GetComponent<Player>().DamageFromEnemy();
      Destroy(gameObject);
    }
    else if (collision.CompareTag("Bullet"))
    {
      Destroy(collision.gameObject);
      Destroy(gameObject);
    }
    else if (collision.CompareTag("ScreenOutOfBounds"))
    {
      Destroy(gameObject);
    }
  }
}