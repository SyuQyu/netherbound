﻿using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageableHit;
    private Animator animator;
    [SerializeField] private int _maxHealth = 100;

    public int MaxHealth
    {
        get { return _maxHealth; }
        set { _maxHealth = value; }
    }

    [SerializeField] private int _health = 100;

    public int Health
    {
        get { return _health; }
        set
        {
            _health = value;

            // If health drops below 0, character is no longer alive
            if (_health <= 0)
            {
                IsAlive = false;
            }
        }
    }

    [SerializeField] private bool _isAlive = true;

    [SerializeField] private bool isInvicible = false;

    private float timeSinceHit = 0f;
    public float invincibilityTime = 0.25f;

    public bool IsAlive
    {
        get { return _isAlive; }
        set
        {
            _isAlive = value;
            animator.SetBool(AnimationStrings.isAlive, value);
        }
    }
    
    /*
     * The velocity should not be changed while this is true but needs to respected by other physics components
     * like the player controller
     */
    public bool LockVelocity
    {
        get { return animator.GetBool(AnimationStrings.lockVelocity); }
        set { animator.SetBool(AnimationStrings.lockVelocity, value); }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isInvicible)
        {
            if (timeSinceHit > invincibilityTime)
            {
                // Remove invincibility
                isInvicible = false;
                timeSinceHit = 0;
            }

            timeSinceHit += Time.deltaTime;
        }
    }

    // Mengembalikan nilai apakah damageable kena serangan atau tidak
    public bool Hit(int damage, Vector2 knocback)
    {
        if (IsAlive && !isInvicible)
        {
            Health -= damage;
            isInvicible = true;

            // Notify other subscribed components that damageable was hit to handle the knockback and such
            animator.SetTrigger(AnimationStrings.hitTrigger);
            LockVelocity = true;
            damageableHit?.Invoke(damage, knocback);
            CharacterEvents.characterDamaged.Invoke(gameObject, damage);

            return true;
        }

        return false;
    }
}