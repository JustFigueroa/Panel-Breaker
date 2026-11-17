using UnityEngine;
public class PlayerAttack : MonoBehaviour
{
    public GameObject meleeAttackHitbox;
    private PlayerController playerController;
    private Animator playerAnim;
    public float attackDuration = .5f;
    public float arielAttackDuration = .5f;
    private float attackTimer;
    private bool isAttacking = false;
    private bool isArielAttacking = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GetComponentInParent<PlayerController>();
        playerAnim = GetComponentInParent<Animator>();
        playerAnim.SetBool("isAttacking", false);
    }

    // Update is called once per frame
    void Update()
    {
    if (Input.GetKeyDown(KeyCode.Mouse0) && !isAttacking && playerController.GetIsGrounded())
        {
            meleeAttackHitbox.SetActive(true);
            attackTimer = attackDuration;
            isAttacking = true;
            playerAnim.SetBool("isAttacking", true);

        }   
    else if  (Input.GetKeyDown(KeyCode.Mouse0) && !isAttacking && !playerController.GetIsGrounded())
        {
            meleeAttackHitbox.SetActive(true);
            attackTimer = attackDuration;
            isArielAttacking = true;
            playerAnim.SetBool("isArielAttacking", true);
        }
    if (isAttacking || isArielAttacking)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0f)
            {
                meleeAttackHitbox.SetActive(false);
                isAttacking = false;
                playerAnim.SetBool("isAttacking", false);
                playerAnim.SetBool("isArielAttacking", false);
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
