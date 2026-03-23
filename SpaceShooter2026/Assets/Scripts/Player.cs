using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
  // set in inspector
  public float speed = 0.1f;
  public GameObject bulletPrefab;
  public Transform bulletSpawnPoint;
  public Slider sliderHealth;
  public Shield shield;
  public GameObject expoPrefab;
  public UI ui;
  public AudioClip clipNormalFire;
  public AudioClip clipSuperFire;
  public AudioClip clipHurt;
  public AudioClip clipPowerupReceived;

  // private fields
  private AudioSource audioSrc;
  private float health;
  private const float Y_LIMIT = 4.6f;

  private void Start() {
    health = 1.0f;
    audioSrc = GetComponent<AudioSource>();
  }

  private void Update() {
    sliderHealth.value = health;

    if (SpaceShooterInput.Instance.input.Fire.WasPressedThisFrame()) {
      GameObject bulletObj = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
      audioSrc.clip = clipNormalFire;
      audioSrc.Play();
    }
    if (SpaceShooterInput.Instance.input.SuperFire.WasPressedThisFrame()) {
      GameObject bulletObj = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
      bulletObj.GetComponent<Bullet>().speed *= 2;
      Instantiate(bulletPrefab, bulletSpawnPoint.position + Vector3.up * 0.5f, Quaternion.identity);
      Instantiate(bulletPrefab, bulletSpawnPoint.position + Vector3.up * -0.5f, Quaternion.identity);
      audioSrc.clip = clipSuperFire;
      audioSrc.Play();
    }

    var vertMove = SpaceShooterInput.Instance.input.MoveVertically.ReadValue<float>();
    this.transform.Translate(Vector3.up * speed * Time.deltaTime * vertMove);

    if (this.transform.position.y > Y_LIMIT) {
      this.transform.position = new Vector3(transform.position.x, Y_LIMIT);
    }
    else if (this.transform.position.y < -Y_LIMIT) {
      this.transform.position = new Vector3(transform.position.x, -Y_LIMIT);
    }
  }

  public void DamageFromEnemy() {
    if (!shield.IsActive) {
      audioSrc.clip = clipHurt;
      audioSrc.Play();
      health -= 0.25f;
      if (health <= 0) {
        var expoObj = Instantiate(expoPrefab, transform.position, Quaternion.identity);
        Destroy(expoObj, expoObj.GetComponent<ParticleSystem>().main.duration);
        Destroy(gameObject);
        ui.ShowGameOver();
      }
    }
  }

  public void RefillShield() {
    audioSrc.clip = clipPowerupReceived;
    audioSrc.Play();
    shield.FullRefill();
  }
}
