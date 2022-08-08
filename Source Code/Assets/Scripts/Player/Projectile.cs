using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed; // Allows you to set the speed in Unity.
    private bool projectileHit;
    private float direction;
    private float lifetime; // Resets fireballs after x second so they don't fly off screen infinitely. 

    private BoxCollider2D boxCollider2D;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (projectileHit) return;
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);

        lifetime += Time.deltaTime;
        if (lifetime > 5) gameObject.SetActive(false); // Resets fireball when x amount of time has passed.
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        projectileHit = true;
        boxCollider2D.enabled = false;
        animator.SetTrigger("ProjectileExplode");

        if (collision.tag == "Enemy")
            collision.GetComponent<Health>().TakeDamage(1);
    }

    public void SetDirection(float _direction) // _direction - underscore used so its not the same name as the local variable.
    {
        lifetime = 0; // Resets lifetime to 0.
        direction = _direction;
        gameObject.SetActive(true);
        projectileHit = false;
        boxCollider2D.enabled = true;

        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != _direction)
            localScaleX = -localScaleX;

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    private void Deactivate() // Deactivates the projectile once the impact animation has finished.
    {
        gameObject.SetActive(false);
    }
}
