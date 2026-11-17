using UnityEngine;
public class PlayerAttack : MonoBehaviour
{
    public GameObject meleeAttackHitbox;
    public float attackDuration = .5f;
    private float attackTimer;
    private bool isAttacking;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isAttacking = false;
    }

    // Update is called once per frame
    void Update()
    {
    if (Input.GetKeyDown(KeyCode.Mouse0) && !isAttacking)
        {
            meleeAttackHitbox.SetActive(true);
            attackTimer = attackDuration;
            isAttacking = true;
        }   
    if (isAttacking)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0f)
            {
                meleeAttackHitbox.SetActive(false);
                isAttacking = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyControl enemy = other.GetComponent<EnemyControl>();
            if (enemy != null)
            {
                enemy.TakeDamage();
            }
        }
    }
}
