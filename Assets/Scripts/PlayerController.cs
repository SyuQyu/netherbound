using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]
public class PlayerController : MonoBehaviour
{
    public float runSpeed = 5f;
    public float jumpImpulse = 10f;
    public float airWalkSpeed = 3f;
    
    [SerializeField] private AudioSource footstepAudioSource; // Komponen AudioSource untuk audio footsteps
    public AudioClip footstepSound; // Suara footsteps
    private float footstepDelay = 0.35f; // Jeda antara pemutaran suara footsteps
    private float nextFootstepTime = 0f; // Waktu berikutnya untuk memainkan suara footsteps
    
    private bool canDash = true;
    [SerializeField] private bool isDashing;
    private float dashingPower = 10f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 3f;

    private Vector2 moveInput;
    private TouchingDirections touchingDirections;
    private Damageable damageable;
    
    [SerializeField] private bool _isMoving = false;

    [SerializeField] private TrailRenderer tr;

    private Rigidbody2D rb;
    private Animator animator;
    
    public float heavyAttackChargeTime = 2f; // Waktu penahanan tombol Q sebelum serangan berat
    private bool isChargingHeavyAttack = false;
    private float currentChargeTime = 0f;
    
    private float moveDistance; // Jarak yang akan ditempuh dalam setiap langkah maju
    private float moveDuration; // Durasi setiap langkah maju
    private int totalSteps; // Jumlah total langkah maju
    
    public bool CanMove
    {
        get { return animator.GetBool(AnimationStrings.canMove); }
    }
    
    public bool CanDash
    {
        get { return canDash; }
    }

    public bool IsAlive
    {
        get { return animator.GetBool(AnimationStrings.isAlive); }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        damageable = GetComponent<Damageable>();
    }

    //Biar ga stuck di tembok pas loncat
    public float CurrentMoveSpeed
    {
        get
        {
            if (!CanMove || touchingDirections.IsOnWall)
            {
                return 0f;
            }

            return touchingDirections.IsGrounded ? runSpeed : airWalkSpeed;
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
    
    public bool IsDashing
    {
        get { return isDashing; }
        private set
        {
            isDashing = value;
            animator.SetBool(AnimationStrings.isDashing, value);
        }
    }
    
    public bool IsChargingHeavyAttack
    {
        get { return isChargingHeavyAttack; }
        private set
        {
            isChargingHeavyAttack = value;
            animator.SetBool(AnimationStrings.isChargingHeavyAttack, value);
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
        if (IsMoving && CanMove && touchingDirections.IsGrounded && !IsDashing && IsAlive)
        {
            if (Time.time >= nextFootstepTime)
            {
                footstepAudioSource.clip = footstepSound;
                footstepAudioSource.Play();
                nextFootstepTime = Time.time + footstepDelay;
            }
        }
        
        if (isChargingHeavyAttack)
        {
            currentChargeTime += Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (IsDashing) return;

        if (!damageable.LockVelocity)
            rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);

        if (!damageable.IsAlive)
        {
            SceneManager.LoadScene("Game Over");
        }

        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (IsAlive)
        {
            IsMoving = moveInput != Vector2.zero;

            SetFacingDirection(moveInput);
        }
        else
        {
            IsMoving = false;
        }
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
        if (context.started && touchingDirections.IsGrounded && CanMove)
        {
            animator.SetTrigger(AnimationStrings.jumpTrigger);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
        }
    }

    public void onAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.attackTrigger);

            if (!IsDashing && touchingDirections.IsGrounded && IsAlive)
            {
                moveDistance = 0.33f; // Jarak yang akan ditempuh dalam setiap langkah maju
                moveDuration = 0.1f; // Durasi setiap langkah maju
                totalSteps = 1; // Jumlah total langkah maju
                StartCoroutine(MovePlayerForward(moveDistance, moveDuration, totalSteps));
            }
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started && CanDash && IsAlive && !IsDashing)
        {
            StartCoroutine(Dash());
        }
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }
    
    public void OnHeavyAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsChargingHeavyAttack = true;
            animator.SetTrigger(AnimationStrings.heavyAttackTrigger);
            animator.SetBool(AnimationStrings.isChargingHeavyAttack, IsChargingHeavyAttack);
            currentChargeTime = 0f;
        }
        else if (context.canceled)
        {
            if (isChargingHeavyAttack)
            {
                if (currentChargeTime >= heavyAttackChargeTime)
                {
                    animator.SetTrigger(AnimationStrings.heavyAttackTrigger);
                    moveDistance = 0.25f; // Jarak yang akan ditempuh dalam setiap langkah maju
                    moveDuration = 0.0625f; // Durasi setiap langkah maju
                    totalSteps = 13; // Jumlah total langkah maju
                    StartCoroutine(MovePlayerForward(moveDistance, moveDuration, totalSteps));
                }
                
                IsChargingHeavyAttack = false;
                animator.SetBool(AnimationStrings.isChargingHeavyAttack, IsChargingHeavyAttack);
                currentChargeTime = 0f; 
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Soul"))
        {
            Destroy(other.gameObject);
            SoulManager.AddSoul();
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        IsDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);

        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;

        rb.gravityScale = originalGravity;
        IsDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
    
    private IEnumerator MovePlayerForward(float moveDistance, float moveDuration, int totalSteps)
    {
        Vector2 startPosition = transform.position; // Posisi awal player
        
        for (int i = 0; i < totalSteps; i++)
        {
            // Menghitung posisi target untuk langkah maju saat ini
            Vector2 targetPosition = startPosition + new Vector2(moveDistance * (i + 1) * (IsFacingRight ? 1f : -1f), 0f);

            // Bergerak secara langsung ke posisi target dengan durasi moveDuration
            yield return MoveToPosition(targetPosition, moveDuration);
        }

        // Menghentikan pergerakan player setelah selesai langkah maju
        rb.velocity = Vector2.zero;
    }
    
    private IEnumerator MoveToPosition(Vector2 targetPosition, float duration)
    {
        float elapsedTime = 0f;
        Vector2 startingPosition = transform.position;

        while (elapsedTime < duration)
        {
            // Menghitung persentase kemajuan pergerakan
            float t = elapsedTime / duration;

            // Menginterpolasi posisi player antara startingPosition dan targetPosition
            Vector2 newPosition = Vector2.Lerp(startingPosition, targetPosition, t);

            // Menggerakkan player ke posisi baru secara langsung
            rb.position = newPosition;

            // Mengupdate waktu yang telah berlalu
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // Mengatur posisi player secara akurat ke targetPosition setelah durasi selesai
        // Menggerakkan player ke posisi baru secara langsung
        rb.position = targetPosition;
    }
}