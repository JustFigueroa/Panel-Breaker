using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private float health = 100; //Player health, when 0 player dies game over or something
    //BASIC MOVEMENT VARIABLES//
    float moveInput;
    public float moveSpeed = 5f; //Speed the palyer moves left and right
    public float footstepInterval = 0.5f; // Interval between footstep sounds
    private float footstepTimer = 0f; // Timer for footstep sounds
    //END BASIC MOVEMENT VARIABLES//
    //START DASH VARIABLES//
    public float dashSpeed; //Speed player dashes in. Varies for horizontal / Vertical
    public float horizontalDashSpeed = 15f; //Speed player dashes horizontally
    public float verticalDashSpeed = 10f; //Speed player dashes vertically
    public float dashDuration = 0.15f; //Duration of the dash in seconds
    public float doubleTapTime = .25f; //Time window for double tap input to trigger dash
    private float dashTimer; //Timer for dash duration. when > 0 player is dashing and cant move
    private float lastLeftPress = -1f; //Time of last left key press
    private float lastRightPress = -1f; //Time of last right key press
    private float lastUpPress = -1f; //Time of last up key press
    private Vector2 dashDirection; //Direction player dashes in, set by double tap input
    //END DASH VARIABLES//
    //START KNOCKBACK VARIABLES//
    private float KnockbackDirectionX; // Direction player is pushed when hit by hazard / enemy X
    private float KnockbackDirectionY; // Direction player is pushed when hit by hazard / enemy Y
    public float knockbackTimer = 1f; //Time player is knocked back and cant move after being hit by hazard / enemy
    public float KnockBackForce = 5f; //Force applied when hit by hazard / enemy
    //END KNOCKBACKVARIABLES//
    //START WALLSLIDE VARIABLES//
    public float wallSlideSpeed = 2f;
    //END WALLSLIDE VARIABLES
    //START JUMP VARIABLES//
    public float jumpForce = 5f; //Force applied when player jumps
    private int thisJump = 2; //Number of possible player jumps. used for double jump feature
    //END JUMP VARIABLES//

    //Bools used for verification checks and animation purposes;
    private bool isGrounded, isDashing, isRunning, isSliding, isKnockedback, isDead;  
    private bool canDashHor, canDashVert, wallOnLeft, wallOnRight;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("WallOnLeft" + wallOnLeft);
        Debug.Log("WallOnRight" + wallOnRight);
        Debug.Log("isSliding" + isSliding);


        if (HandleHealthCheck()) return;
        if (HandleKnockbackCheck()) return;
        if (HandleDashCheck()) return;
        
        HandleInput();
        HandleWallSlide();
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        //Ground check used for jump feature and animation purposes also resets jump counter
        if (other.CompareTag("Ground"))
        {
            isGrounded = true;
            thisJump = 2;
            canDashHor = true;
            canDashVert = true;
        }
        //hazard check used for knockback and health reduction
        if (other.CompareTag("Hazard") || other.CompareTag("Enemy"))
        {
            health -= 20;
            if (transform.position.x > other.transform.position.x)
                KnockbackDirectionX = 1f;
            else
                KnockbackDirectionX = -1f;
            if (transform.position.y > other.transform.position.y)
                KnockbackDirectionY = 1f;
            else
                KnockbackDirectionY = -1f;
            pushBackPlayer(KnockbackDirectionX, KnockbackDirectionY);
        }
        if (other.gameObject.CompareTag("Wall") && !isGrounded){
            isSliding = true;
            if (thisJump <= 0)
                thisJump++;
            if (other.bounds.center.x < transform.position.x)
            {
                wallOnLeft = true;
            }
            else
            {
                wallOnRight = true;
            }
        }
    }



    void OnTriggerExit2D(Collider2D other){
        if (other.CompareTag("Ground")){
           isGrounded = false;
        }
        if (other.CompareTag("Wall")){
            isSliding = false;
            wallOnLeft = false;
            wallOnRight = false;
        }
    }

    private void pushBackPlayer(float directionX, float directionY){
        PlayerAudioManager.instance.PlaySound(PlayerAudioManager.instance.hurtSound);
        PlayerAudioManager.instance.PlaySound(PlayerAudioManager.instance.powSound);

        rb.linearVelocity = new Vector2(KnockbackDirectionX * KnockBackForce, KnockbackDirectionY * KnockBackForce);
        knockbackTimer = 0.5f;
    }
    private void StartDash(Vector2 direction){
        if (dashTimer > 0f)
        {
            return;
        }
        PlayerAudioManager.instance.PlaySound(PlayerAudioManager.instance.dashGruntSound);
        //PlayerAudioManager.instance.PlaySound(PlayerAudioManager.instance.dashSound);
        dashDirection = direction;
        dashTimer = dashDuration;

        if (direction == Vector2.up){
            dashSpeed = verticalDashSpeed;
        }
        else{
            dashSpeed = horizontalDashSpeed;
        }
    }

    private void HandleInput()
    {
        //Horizontal Movement
        moveInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        //Dash Logic
        if (Input.GetKeyDown(KeyCode.A) && canDashHor)
        {
            if (Time.time - lastLeftPress <= doubleTapTime)
            {
                StartDash(Vector2.left);
                lastLeftPress = -1f;
                canDashHor = false;
            }
            else
            {
                lastLeftPress = Time.time;
            }
        }

        if (Input.GetKeyDown(KeyCode.D) && canDashHor){
            if (Time.time - lastRightPress <= doubleTapTime)
            {
                StartDash(Vector2.right);
                lastRightPress = -1f;
                canDashHor = false;

            }
            else
            {
                lastRightPress = Time.time;
            }
        }

        if (Input.GetKeyDown(KeyCode.W) && canDashVert)
        {
            if(Time.time - lastUpPress <= doubleTapTime)
            {
                StartDash(Vector2.up);
                lastUpPress = -1f;
                canDashVert = false;
            }
            else
            {
                lastUpPress = Time.time;
            }
        }
        
        //Jump Logic
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            PlayerAudioManager.instance.PlaySound(PlayerAudioManager.instance.jumpGruntSound);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            thisJump--;
        }

        //Double Jump Feature, maybe pickup item can increase this value 
        else if (Input.GetKeyDown(KeyCode.Space) && !isGrounded && thisJump > 0)
        {
            PlayerAudioManager.instance.PlaySound(PlayerAudioManager.instance.jumpGruntSound);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            thisJump--;
        }

        //Run logic for sound and anim
        if (Mathf.Abs(rb.linearVelocity.x) > 0.01f && isGrounded)
            isRunning = true;
        else isRunning = false;

        if (isRunning)
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0f)
            {
                PlayerAudioManager.instance.PlaySound(PlayerAudioManager.instance.runSound);
                footstepTimer = footstepInterval;
            }
        }
        else footstepTimer = 0f;


    }

    private bool HandleHealthCheck()
    {        
        if (health <= 0)
        {
        Destroy(this);
        isDead = true;
        }
        else isDead = false;
        return isDead;
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
    private bool HandleDashCheck()
    {

        if (dashTimer > 0)
        {
            dashTimer -= Time.deltaTime;
            rb.linearVelocity = dashDirection * dashSpeed;
            isDashing = true;
        } 
        else isDashing = false;
        return isDashing;
    }
    private void HandleWallSlide()
    {
        if (!isSliding)
        return;
        if (rb.linearVelocity.y < -wallSlideSpeed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, - wallSlideSpeed);
        }
        if (wallOnLeft && moveInput < 0f)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        }
        if (wallOnRight && moveInput > 0f)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        }

    }
}
