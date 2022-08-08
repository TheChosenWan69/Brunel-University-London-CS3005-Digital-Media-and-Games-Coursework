using UnityEngine;

public class meleeEnemy : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCD;
    [SerializeField] private float range;
    [SerializeField] private int damage;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider2D;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;

    [Header("Sound Clip")]
    [SerializeField] private AudioClip soundClip;

    //References
    private Animator animator;
    private Health playerHealth;
    private EnemyRoam enemyRoam;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemyRoam = GetComponentInParent<EnemyRoam>();
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (PlayerInRange()) // Attacks when player is within the aggro range of the mob.
        {
            if (cooldownTimer >= attackCD && playerHealth.currentHealth > 0)
            {
                cooldownTimer = 0;
                animator.SetTrigger("Attack");
                SoundManager.instance.PlaySound(soundClip);
            }
        }

        if (enemyRoam != null)
            enemyRoam.enabled = !(PlayerInRange());
    }

    private bool PlayerInRange()
    {
        RaycastHit2D hit = // Aggro range hitbox.
            Physics2D.BoxCast(boxCollider2D.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider2D.bounds.size.x * range, boxCollider2D.bounds.size.y, boxCollider2D.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
            playerHealth = hit.transform.GetComponent<Health>();

        return hit.collider != null;
    }
    private void OnDrawGizmos() // Draws the hitbox for reference via Gizmo.
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider2D.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider2D.bounds.size.x * range, boxCollider2D.bounds.size.y, boxCollider2D.bounds.size.z));
    }

    private void DamagePlayer()
    {
        if (PlayerInRange())
            playerHealth.TakeDamage(damage);
    }
}
