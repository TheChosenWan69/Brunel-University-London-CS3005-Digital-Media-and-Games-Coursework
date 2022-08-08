using UnityEngine;

public class enemyProjectileHolder : MonoBehaviour
{
    [SerializeField] private Transform enemy; // Transform projectile depending on where the enemy is facing.

    private void Update()
    {
        transform.localScale = enemy.localScale;
    }
}