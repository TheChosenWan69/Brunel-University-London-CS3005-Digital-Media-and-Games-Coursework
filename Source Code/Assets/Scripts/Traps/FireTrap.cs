using UnityEngine;
using System.Collections;

public class FireTrap : MonoBehaviour
{
    [SerializeField] private float damage;

    [Header("Firetrap Timers")]
    [SerializeField] private float activationDelay;
    [SerializeField] private float activeTime;
    private Animator animator;
    private SpriteRenderer rend;

    private bool triggered; // Fire Trap trigger.
    private bool active; // Fire Trap active.

    private Health player;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!triggered)
                StartCoroutine(ActivateFiretrap());

            player = collision.GetComponent<Health>();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        player = null;
    }

    private void Update()
    {
        if (active && player != null)
            player.TakeDamage(damage);
    }

    private IEnumerator ActivateFiretrap()
    {
        triggered = true;
        rend.color = Color.red; // Visual indicator

        yield return new WaitForSeconds(activationDelay); // Time delay for trap to activate, animate and return back to normal.
        rend.color = Color.white; // Reverts sprite back to normal colour.
        active = true;
        animator.SetBool("Active", true);

        yield return new WaitForSeconds(activeTime); // Wait until activeTime seconds to be over to deactivate and resets everything.
        active = false;
        triggered = false;
        animator.SetBool("Active", false);
    }
}