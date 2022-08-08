using UnityEngine;

public class rangedEnemy : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCD;
    [SerializeField] private float range;
    [SerializeField] private int damage;

    [Header("Ranged Attack")]
    [SerializeField] private Transform firepoint;
    [SerializeField] private GameObject[] swordSlash;

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
            if (cooldownTimer >= attackCD)
            {
                cooldownTimer = 0;
                animator.SetTrigger("rangedAttack");
                SoundManager.instance.PlaySound(soundClip);
            }
        }

        if (enemyRoam != null)
            enemyRoam.enabled = !(PlayerInRange());
    }

    private void RangedAttack()
    {
        cooldownTimer = 0;
        swordSlash[FindswordSlash()].transform.position = firepoint.position;
        swordSlash[FindswordSlash()].GetComponent<EnemyProjectile>().ActivateProjectile();
    }
    private int FindswordSlash()
    {
        for (int i = 0; i < swordSlash.Length; i++)
        {
            if (!swordSlash[i].activeInHierarchy)
                return i;
        }
        return 0;
    }

    private bool PlayerInRange() // Aggro range hitbox.
    {
        RaycastHit2D hit =
            Physics2D.BoxCast(boxCollider2D.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider2D.bounds.size.x * range, boxCollider2D.bounds.size.y, boxCollider2D.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        return hit.collider != null;
    }
    private void OnDrawGizmos() // Draws the hitbox for reference via Gizmo.
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider2D.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider2D.bounds.size.x * range, boxCollider2D.bounds.size.y, boxCollider2D.bounds.size.z));
    }
}
