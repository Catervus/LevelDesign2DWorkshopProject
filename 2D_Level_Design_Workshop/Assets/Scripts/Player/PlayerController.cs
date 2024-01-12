using System.Collections;
using UnityEngine;

public enum PlayerState
{
    walking,
    attacking,
    hit,

}

#pragma warning disable 0649

public class PlayerController : MonoBehaviour
{
    #region variables

    #region movement-variables
    
    [SerializeField] [Range (0,20)] [Tooltip("The speed with which the player moves")]
    private float moveSpeed; // movementspeed
    [SerializeField] [Range(0, 30)] [Tooltip("The force with which the player jumps")]
    private float jumpForce; // force applied when jumping
    [SerializeField] [Range(0, 10)] [Tooltip("Downward force when jumping but not holding Jump")]
    private float lowJumpMultiplier; // downward force applied when jumping 
                                     // to ensure the player reaches higher when holding the jump button
    [SerializeField] [Range (0,0.3f)] [Tooltip("The time the player can jump after leaving a plattform")]
    private float coyoteTimer; // player can jump a little while after not being grounded anymore
    private float currentCoyoteTimer;
    [SerializeField] [Range(0, 20)] [Tooltip ("The Speed with which the player climbs a ladder")]
    private float ladderClimbSpeed; // movementspeed when climbing ladders
    private bool canDoubleJump;
    private float xInput; // players x input
    private float yInput; // players y inpit
    //private bool isGrounded; // is true if player is grounded, false when player is not grounded
    private bool shouldJump;
    private bool isClimbingLadder;

    #endregion

    [Space(30)]

    #region combat-variables
    [SerializeField] [Range(0, 2)] [Tooltip("Cooldown to attack again after attacking")]
    private float attackCooldown; // cooldown stat of attack move
    private float currentAttackCooldown; // 
    [SerializeField] [Tooltip("Attack Box Dimensions ")]
    private Vector2 attackBoxDimension; // dimensions of the box that checks if player hits an enemy
    [SerializeField] [Tooltip("Position of the attack where the Attack Hit Box is cast")]
    public Transform attackPos; // position on which the attack box-collider is checked
    #endregion

    [Space(30)]

    #region other variables

    [SerializeField] [Tooltip("The Ground Hit Box")]
    private Vector2 checkBoxDimensions; // dimensions of the box that checks if player is grounded
    [SerializeField] [Tooltip("The Ladder Hit Box")]
    private Vector2 ladderBoxDimensions; // dimensions of the box that checks if player is on a ladder
    [SerializeField] [Tooltip("Position of the feet where the Ground Hit Box is cast")]
    private Transform feetPos; // position used for checking if player is grounded
    [SerializeField] [Tooltip("Ground Layer")]
    private LayerMask groundLayer; // layer of ground objects in the world
    [SerializeField] [Tooltip("Ladder Layer")]
    private LayerMask ladderLayer; // layer of ladder objects in the world
    [SerializeField] [Tooltip("Enemy Layer")]
    private LayerMask enemyLayer; // layer of enemy objects in the world
    [SerializeField] [Tooltip("Object Layer")] 
    private LayerMask objectLayer; // layer of enemy objects in the world

    private bool facingRight = true; // used for flipping the player
    private bool canInteract = true; // if the player opened the PauseMenu or collided with a AreaTransitioner (which loads a new Scene)
                                     // the player shouldn't be able to input for example an attack

    #endregion
    

    [HideInInspector]
    public PlayerState currentState = PlayerState.walking; // current Player State

    [Space(30)]

    // components
    private Rigidbody2D rb;
    private Animator anim;
    private ParticleSystem.EmissionModule particleEmission;
    [SerializeField] [Tooltip("Rate at which particles are emitted")]
    private float particleEmissionRate;


    #endregion


    #region Methods

    private void Start()
    {
        GetPlayerComponents();

    }

    /// <summary>
    /// gets all players components, e.g. Rigidbody2D
    /// </summary>
    private void GetPlayerComponents()
    {
        rb = GetComponent<Rigidbody2D>();
        //anim = GetComponentInChildren<Animator>();
        //particleEmission = GetComponentInChildren<ParticleSystem>().emission;
    }

    private void Update()
    {
        GetPlayerInput();

        // Animations();

        if (GroundCheck())
            currentCoyoteTimer = coyoteTimer;

        currentCoyoteTimer -= Time.deltaTime;
    }


    /// <summary>
    /// In fixed update all physics calculations are handled
    /// </summary>
    private void FixedUpdate()
    {
        if (!canInteract) // if the player shouldn't be able to interact -> return 
            return;

        if (isClimbingLadder) // if player is currently on a ladder 
            rb.velocity = new Vector2(xInput * (ladderClimbSpeed / 2), 
                Vector2.up.y * ladderClimbSpeed); // player moves Up with ladderClimbSpeed and moves Sidewards with half of
                                                 // ladderClimbSpeed
                                                                                                            
        else // player sidewards movement velocity
            if(currentState == PlayerState.walking) // if player is currently not attacking or hit
                rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y); // x velocity is changed by xInput multiplied with moveSpeed


        if (currentState == PlayerState.walking) // if player is not for example attacking
        {                                       // results in an input buffer, should feel better for the player
            if (shouldJump)
            {
                Jump();
                shouldJump = false;
            }
            BetterJump();
        }
        else
        {
            if (Input.GetButtonUp("Jump")) // if player presses jump when not able to
                shouldJump = false;                                                   //  jump but releases before able to jump
                                                                                     // input is not buffered
        }
    }

    /// <summary>
    /// changes when Player is able to move/attack
    /// called when for example the player opened the PauseMenu or collided with a AreaTransitioner (which loads a new Scene)
    /// </summary>
    /// <param name="_movestate"></param>
    public void CanMoveSwitch(bool _movestate)
    {
        canInteract = _movestate;
    }

    /// <summary>
    /// collects players movement Input
    /// </summary>
    private void GetPlayerInput()
    {
        if (!canInteract) // if the player shouldn't be able to interact -> return 
            return;

        if (currentState == PlayerState.walking) // if player is currently not attacking or hit
        {
            xInput = Input.GetAxisRaw("Horizontal");
            yInput = Input.GetAxisRaw("Vertical");

        }
        else // If player is attacking or hit x and y Input are reset
        {
            xInput = 0;
            yInput = 0;
        }

        if (Input.GetButtonDown("Jump")) // checks for jumpinput
        {
            JumpRequest();
        }

        // if (yInput > 0)
        // {
        //     if (LadderCheck()) // checks if a Ladder is behind the player
        //         isClimbingLadder = true; // if yes -> the player is climbing the ladder
        //     else
        //         isClimbingLadder = false; // if no -> player is not climbing the ladder
        // }
        // else
        // {
        //     isClimbingLadder = false; // if yInput is negativ or zero player is not climbing a ladder
        // }



        // if (currentAttackCooldown <= 0)
        // {
        //     if (Input.GetButtonDown("Attack"))
        //     {
        //         if(GroundCheck()) // Player is not able to attack in the air
        //         {
        //             Attack();
        //             currentAttackCooldown = attackCooldown; // attack cooldown is reset
        //         }
        //         
        //     }
        // }
        // else // if attack cooldown is not up yet
        // {
        //     currentAttackCooldown -= Time.deltaTime; // it is decreased by Time.deltaTime
        // }


    }



    #endregion

    #region Jumping

    /// <summary>
    /// checks if player is able to jump and enables jump
    /// </summary>
    private void JumpRequest()
    {
        //if (GroundCheck() || canDoubleJump) // if player is grounded or can double jump
        //{
        //    shouldJump = true;
        //}

        if (currentCoyoteTimer > 0 || canDoubleJump) // if player is grounded or can double jump
        {
            shouldJump = true;
        }
    }

    /// <summary>
    /// jump mechanic
    /// </summary>
    private void Jump()
    {
        canDoubleJump = false;
        rb.velocity = new Vector2(rb.velocity.x, 0); // vertical velocity is reset
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); // force is added
    }

    /// <summary>
    /// if player holds the jump button he reaches higher
    /// </summary>
    private void BetterJump()
    {
        if (rb.velocity.y > 0.1 && !Input.GetButton("Jump") && isClimbingLadder == false) // if player is mid-jump but not holding jump button
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * lowJumpMultiplier * Time.deltaTime; // gravity influence is elevated
        }
    }

    /// <summary>
    /// is called when player collides with jump-reset-diamond, allows player to double jump
    /// </summary>
    public void ActivateDoubleJump()
    {
        canDoubleJump = true;
    }



    /// <summary>
    /// checks if player is grounded
    /// </summary>
    /// <returns>Returns true if player is grounded, returns false if player is not grounded</returns>
    private bool GroundCheck()
    {
        Collider2D checkBox = Physics2D.OverlapBox(feetPos.position, checkBoxDimensions, 1, groundLayer); // checkBox is 
                                                                                                         // cast on Players feet position
        if (checkBox) // is grounded
        {
            return true; 
        }
        // is not grounded
        return false;
    }

    /// <summary>
    /// checks for a Ladder on players position
    /// </summary>
    /// <returns>Returns true if there is a Ladder, returns false if not</returns>
    private bool LadderCheck()
    {
        Collider2D checkBox = Physics2D.OverlapBox(transform.position, ladderBoxDimensions, 1, ladderLayer);
        if (checkBox)
            return true;
        else
            return false;
    }

    #endregion

    #region Animation

    /// <summary>
    /// handles 'every-frame-animations'
    /// </summary>
    private void Animations()
    {
        Flip();

        if (xInput != 0)
            anim.SetBool("isWalking", true);
        else
            anim.SetBool("isWalking", false);

        if (rb.velocity == Vector2.zero)
            particleEmission.rateOverTime = 0;
        else if (rb.velocity.y < 0.1f && rb.velocity.y > -0.1f)
            particleEmission.rateOverTime = particleEmissionRate;
        else
            particleEmission.rateOverTime = 0;




        // anim.SetFloat("yVelocity", rb.velocity.y);


        if (GroundCheck()) 
        {
            anim.SetBool("isJumping", false);
            anim.SetBool("isFalling", false);
        }
        else
        {
            if (rb.velocity.y > 0)
            {
                anim.SetBool("isJumping", true);
                anim.SetBool("isFalling", false);
            }
            else if (rb.velocity.y < 0)
            {
                anim.SetBool("isFalling", true);
                anim.SetBool("isJumping", false);
            }
            else
            {
                anim.SetBool("isJumping", false);
                anim.SetBool("isFalling", false);
            }
        }
        
            
    }

    /// <summary>
    /// flips the player when changing x-walk-direction
    /// </summary>
    private void Flip()
    {
        if (xInput > 0) // if Input is to walk right 
        {
            if (!facingRight) // and player is facing left
            {
                transform.Rotate(0, 180, 0); // player is rotated to the right
                // transform.localScale = new Vector3(transform.localScale.x * - 1, 1, 1);
                facingRight = true;
            }
        }
        if (xInput < 0) // if Input is to walk left
        {
            if (facingRight) // and player is facing right
            {
                transform.Rotate(0, 180, 0); // player is rotated to face right
                // transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
                facingRight = false;
            }
        }
    }

    #endregion

    #region Gizmos

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(feetPos.position, checkBoxDimensions); // feet position checkbox
        //Gizmos.DrawWireCube(transform.position, ladderBoxDimensions); // ladder checkbox
        //Gizmos.DrawWireCube(attackPos.position, attackBoxDimension); // attack checkbox
    }

    #endregion


}

#pragma warning restore 0649

