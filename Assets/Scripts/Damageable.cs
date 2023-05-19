﻿using UnityEngine;

public class Damageable : MonoBehaviour
{
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
            if (_health < 0)
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
        
        Hit(10);
    }

    public void Hit(int damage)
    {
        if (IsAlive && !isInvicible)
        {
            Health -= damage;
            isInvicible = true;
        }
    }
}