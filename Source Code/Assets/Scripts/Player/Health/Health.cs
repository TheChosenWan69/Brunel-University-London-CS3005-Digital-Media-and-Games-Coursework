using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    [Header ("HP")]
    [SerializeField] private float startHealth;

    [Header("Invincibility Frames")]
    [SerializeField] private float iFrameDuration;
    [SerializeField] private float noOfFlashes;

    [Header("Components")]
    [SerializeField] private Behaviour[] components;

    [Header("Sound Clip")]
    [SerializeField] private AudioClip deathClip;
    [SerializeField] private AudioClip hitClip;

    public float currentHealth { get; private set; } // { get; private set; } 'get' = get this variable from any script, 'private set' = currentHealth can only be changed inside this script.
    private Animator animator;
    private bool dead;
    private SpriteRenderer rend; // Function to make sprite flash on damage.
    private bool invuln;


    private void Awake()
    {
        currentHealth = startHealth;
        animator = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float _damage)
    {
        if (invuln) return;
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startHealth);

        if (currentHealth > 0)
        {
            animator.SetTrigger("Hurt");
            StartCoroutine(Invuln());
            SoundManager.instance.PlaySound(hitClip);
        }
        else
        {
            if (!dead)
            {
                animator.SetTrigger("Death");

                // Deactivate all attached component classes.
                foreach (Behaviour component in components)
                    component.enabled = false;

                dead = true;
                SoundManager.instance.PlaySound(deathClip);
            }
        }
    }

    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startHealth);
    }

    private IEnumerator Invuln() // Invulnerability timer.
    {
        invuln = true;
        Physics2D.IgnoreLayerCollision(10, 11, true);   
        for (int i = 0; i <noOfFlashes; i++)
        {
            rend.color = new Color(1, 0, 0, 0.5f); // Flashes red.
            yield return new WaitForSeconds(iFrameDuration / (noOfFlashes * 2));
            rend.color = Color.white; // Unity's predefined color code.
            yield return new WaitForSeconds(iFrameDuration / (noOfFlashes * 2));
        }

        Physics2D.IgnoreLayerCollision(10, 11, false);
        invuln = false;
   
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
