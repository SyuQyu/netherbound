using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
public class PlayerController : MonoBehaviour
{
    public float runSpeed = 5f;
    public float jumpImpulse = 10f;
    public float airWalkSpeed = 3f;
    Vector2 moveInput;
    TouchingDirections touchingDirections;

    [SerializeField] private bool _isMoving = false;

    Rigidbody2D rb;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
    }

    //Biar ga stuck di tembok pas loncat
    public float StateSpeed
    {
        get
        {
            if (!touchingDirections.IsOnWall)
            {
                if (touchingDirections.IsGrounded)
                {
                    return runSpeed;
                }
                else
                {
                    //air move
                    return airWalkSpeed;
                }
            }
            else
            {
                return runSpeed;
            }
        }
    }

    public bool IsMoving
    {
        get { return _isMoving; }
        private set
        {
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        }
    }

    public bool _isFacingRight = true;

    public bool IsFacingRight
    {
        get { return _isFacingRight; }
        private set
        {
            if (_isFacingRight != value)
            {
                // Putar skala lokal untuk membuat player menghadap ke arah yg berlawanan
                transform.localScale *= new Vector2(-1, 1);
            }

            _isFacingRight = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput.x * StateSpeed, rb.velocity.y);
        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        IsMoving = moveInput != Vector2.zero;

        SetFacingDirection(moveInput);
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
        {
            // menghadap ke kanan
            IsFacingRight = true;
        }
        else if (this.moveInput.x < 0 && IsFacingRight)
        {
            // menghadap ke kiri
            IsFacingRight = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        // TODO Check if alive as well
        if (context.started && touchingDirections.IsGrounded)
        {
            animator.SetTrigger(AnimationStrings.jump);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
        }
    }
}