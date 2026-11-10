using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; //Speed the palyer moves left and right
    public float jumpForce = 5f; //Force applied when player jumps
    private Vector2 dashDirection;
    public float dashSpeed;
    public float horizontalDashSpeed = 15f;
    public float verticalDashSpeed = 10f;

    public float dashDuration = 0.15f;
    public float doubleTapTime = .25f;
    private float lastLeftPress = -1f;
    private float lastRightPress = -1f;
    private float lastUpPress = -1f;
    private float dashTimer;
    public float knockbackTimer = 1f; //Time player is knocked back and cant move after being hit by hazard / enemy
    public float pushBackForce = 5f; //Force applied when hit by hazard / enemy
    public bool isGrounded;     //Verify player is grounded before jumping, also used for animation purposes
    private int thisJump = 2; //Number of possible player jumps. used for double jump feature
    private float pushbackDirectionX; // Direction player is pushed when hit by hazard / enemy X
    private float pushbackDirectionY; // Direction player is pushed when hit by hazard / enemy Y

    private float health = 100; //Player health, when 0 player dies game over or something
    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Health Check
        if (health <= 0){
            GetComponent<SpriteRenderer>().color = Color.red;
            return;
        }
        //Knockback check
        if (knockbackTimer > 0f){
                knockbackTimer -= Time.deltaTime;
                return;
        }
        //Dash Check
        if (dashTimer > 0){
            dashTimer -= Time.deltaTime;
            rb.linearVelocity = dashDirection * dashSpeed;
            return;
        }  
        

        //Player Movement    
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (Time.time - lastLeftPress <= doubleTapTime)
            {
                StartDash(Vector2.left);
                lastLeftPress = -1f;
            }
            else
            {
                lastLeftPress = Time.time;
            }
        }
        if (Input.GetKeyDown(KeyCode.D)){
            if (Time.time - lastRightPress <= doubleTapTime)
            {
                StartDash(Vector2.right);
                lastRightPress = -1f;
            }
            else
            {
                lastRightPress = Time.time;
            }
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            if(Time.time - lastUpPress <= doubleTapTime)
            {
                StartDash(Vector2.up);
                lastUpPress = -1f;
            }
            else
            {
                lastUpPress = Time.time;
            }
        }

        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded){
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            thisJump--;
        }

        //Double Jump Feature, maybe pickup item can increase this value 
        if (Input.GetKeyDown(KeyCode.Space) && !isGrounded && thisJump > 0){
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            thisJump--;
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        //Ground check used for jump feature and animation purposes also resets jump counter
        if (other.CompareTag("Ground"))
        {
            isGrounded = true;
            thisJump = 2;
        }
        //hazard check used for knockback and health reduction
        if (other.CompareTag("Hazard"))
        {
            health -= 20;
            if (transform.position.x > other.transform.position.x)
                pushbackDirectionX = 1f;
            else
                pushbackDirectionX = -1f;
            if (transform.position.y > other.transform.position.y)
                pushbackDirectionY = 1f;
            else
                pushbackDirectionY = -1f;
            pushBackPlayer(pushbackDirectionX, pushbackDirectionY);
        }
    }

    void OnTriggerExit2D(Collider2D other){
        if (other.CompareTag("Ground")){
           isGrounded = false;
        }
    }
    private void pushBackPlayer(float directionX, float directionY){
        rb.linearVelocity = new Vector2(pushbackDirectionX * pushBackForce, pushbackDirectionY * pushBackForce);
        knockbackTimer = 0.5f;
    }
    private void StartDash(Vector2 direction){
        dashDirection = direction;
        dashTimer = dashDuration;
        if (direction == Vector2.up){
            dashSpeed = verticalDashSpeed;
        }
        else{
            dashSpeed = horizontalDashSpeed;
        }

    }
}
