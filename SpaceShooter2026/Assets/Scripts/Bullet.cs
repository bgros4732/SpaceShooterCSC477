using UnityEngine;

public class Bullet : MonoBehaviour {
  public float speed = 95f;

  private void Update() {
    this.transform.Translate(Vector3.right * speed * Time.deltaTime);
  }

  private void OnTriggerEnter2D(Collider2D collision) {
    if (collision.CompareTag("ScreenOutOfBounds")) {
      Destroy(gameObject);
    }
  }
}
