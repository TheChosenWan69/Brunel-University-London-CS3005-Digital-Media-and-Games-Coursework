using UnityEngine;

public class HealthHearts : MonoBehaviour
{
    [SerializeField] private float healthValue;
    [SerializeField] private AudioClip soundClip;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            SoundManager.instance.PlaySound(soundClip);
            collision.GetComponent<Health>().AddHealth(healthValue);
            gameObject.SetActive(false);
        }
    }
}