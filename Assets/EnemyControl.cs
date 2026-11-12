using UnityEditor;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    private Rigidbody2D rb;
    private float health = 20f;
    //START KNOCKBACK VARIABLES//
    private float KnockbackDirectionX; // Direction player is pushed when hit by hazard / enemy X
    private float KnockbackDirectionY; // Direction player is pushed when hit by hazard / enemy Y
    public float knockbackTimer = 1f; //Time player is knocked back and cant move after being hit by hazard / enemy
    public float KnockBackForce = 5f; //Force applied when hit by hazard / enemy
    private bool isKnockedback;
    //END KNOCKBACKVARIABLES//

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (HandleKnockbackCheck()) return;

    }

    public void TakeDamage()
    {
        health -= 5f;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerHitbox"))
        {
            if (transform.position.x > other.transform.position.x)
                KnockbackDirectionX = 1f;
            else
                KnockbackDirectionX = -1f;
            if (transform.position.y > other.transform.position.y)
                KnockbackDirectionY = 1f;
            else
                KnockbackDirectionY = -1f;
            PushBackEnemy(KnockbackDirectionX, KnockbackDirectionY);
        }
    }
    private bool HandleKnockbackCheck()
    {   
        if (knockbackTimer > 0f){
                knockbackTimer -= Time.deltaTime;
                isKnockedback = true;
        }
        else isKnockedback = false;
        return isKnockedback;
    }
        private void PushBackEnemy(float directionX, float directionY){
        rb.linearVelocity = new Vector2(KnockbackDirectionX * KnockBackForce, KnockbackDirectionY * KnockBackForce);
        knockbackTimer = 0.5f;
    }
    
}
