using UnityEngine;

public class Attack : MonoBehaviour
{
    // [SerializeField] allows us to change value in Unity.  
    [SerializeField] private float attackCD;
    [SerializeField] private Transform projectilePoint;
    [SerializeField] private GameObject[] projectileArray;
    [SerializeField] private AudioClip soundClip;

    private Animator animator;
    private Movement movement;
    private float cdTimer = Mathf.Infinity;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<Movement>(); 
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && cdTimer > attackCD && movement.canAttack())
            playerAttack();

        cdTimer += Time.deltaTime;
    }

    private void playerAttack()
    {
        SoundManager.instance.PlaySound(soundClip);
        animator.SetTrigger("Attack");
        cdTimer = 0;

        projectileArray[findProjectile()].transform.position = projectilePoint.position;
        projectileArray[findProjectile()].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
    }

    private int findProjectile()
    {
        for (int i = 0; i < projectileArray.Length; i++)
        {
            if (!projectileArray[i].activeInHierarchy)
                return i;
        }
        return 0;
    }
}
