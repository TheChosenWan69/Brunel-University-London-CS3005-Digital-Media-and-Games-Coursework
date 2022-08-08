using UnityEngine;
using UnityEngine.SceneManagement;

public class Flag : MonoBehaviour
{
    [SerializeField] private AudioClip soundClip;
    private bool levelCompleted = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player" && !levelCompleted)
        {
            SoundManager.instance.PlaySound(soundClip);
            levelCompleted = true;
            Invoke("CompleteLevel", 1f); // 1 second delay before switching to next level.
        }
    }

    private void CompleteLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Switches to the next scene.
    }
}
